using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
namespace notepad_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentNoteId = -1; // Initialize to -1 to indicate no note is selected
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        
        
        
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            Adding_Button.Visibility = Visibility.Collapsed;
            CardItemsControl.Visibility = Visibility.Collapsed;
            
            Writing_text_window.Visibility = Visibility.Visible;

        }

        private void Title_box_GotFocus(object sender, RoutedEventArgs e)
        {

            if (Title_box.Text == "Title")
            {
                Title_box.Text = "";
                Title_box.Foreground = new SolidColorBrush(Colors.White);
                Title_box.Focus();
                Title_box.Select(Title_box.Text.Length, 0);
                Title_box.CaretBrush = new SolidColorBrush(Colors.White);
            }



        }

        [Obsolete]
        private void Title_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Title_box.FontWeight = FontWeights.Bold;
            FormattedText formattedText = new FormattedText(
                Title_box.Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(Title_box.FontFamily, Title_box.FontStyle, Title_box.FontWeight, Title_box.FontStretch),
                Title_box.FontSize,
                Brushes.White,
                new NumberSubstitution(),
                TextFormattingMode.Display);
            double newHeight = formattedText.Height + 10;
            Title_box.Height = Math.Min(newHeight, 300);
            Title_box.Height = double.NaN;
            Title_box.UpdateLayout();
        }

        private void Text_box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Text_box.Text == "Write a Note")
            {
                Text_box.Text = "";
                Text_box.Foreground = new SolidColorBrush(Colors.White);
                Text_box.Focus();
                Text_box.Select(Title_box.Text.Length, 0);
                Text_box.CaretBrush = new SolidColorBrush(Colors.White);
            }

        }

        string lastSavedTitle = "";
        string lastSavedInput = "";
        private object input;

        private void Save_button(object sender, EventArgs e)
        {
            string dataconnection = "Data Source=DESKTOP-UR59NAB;Initial Catalog=NOTEAPP;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

            string Titles = Title_box.Text.Trim();
            string input = Text_box.Text.Trim();

            if (Titles == lastSavedTitle && input == lastSavedInput)
            {
                GoToMainMenu(sender, (RoutedEventArgs)e);
                return;
            }

            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(dataconnection))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM savenotes WHERE Titles = @Titles";
                using (Microsoft.Data.SqlClient.SqlCommand checkCmd = new Microsoft.Data.SqlClient.SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.Add("@Titles", SqlDbType.NVarChar).Value = Titles;
                    int count = (int)checkCmd.ExecuteScalar();

                    string query;
                    if (count > 0)
                    {
                        query = "UPDATE savenotes SET input = @input WHERE Titles = @Titles";
                    }
                    else
                    {
                        query = "INSERT INTO savenotes (Titles, input) VALUES (@Titles, @input)";
                    }

                    using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Titles", SqlDbType.NVarChar).Value = Titles;
                        cmd.Parameters.Add("@input", SqlDbType.NVarChar).Value = input;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = currentNoteId;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            lastSavedTitle = Titles;
            lastSavedInput = input;

            LoadData();
            Button_save.Content = "S&M";
        }

        private void GoToMainMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainMenu = new MainWindow();
            mainMenu.Show();
            this.Close();
        }

        private void LoadData()
        {
            string dataconnection = "Data Source=DESKTOP-UR59NAB;Initial Catalog=NOTEAPP;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(dataconnection))
            {
                con.Open();
                string query = "SELECT * FROM savenotes ORDER BY LastUpdated DESC";
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con);
                Microsoft.Data.SqlClient.SqlDataAdapter da = new Microsoft.Data.SqlClient.SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CardItemsControl.ItemsSource = dt.DefaultView;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Note_Click(object sender, RoutedEventArgs e)
        {
            Button? clickbutton = sender as Button;
            DataRowView? row = clickbutton.DataContext as DataRowView;
            if (row != null)
            {
                Title_box.Text = row["Titles"].ToString();
                Text_box.Text = row["input"].ToString();
                if (row["Id"] != DBNull.Value)
                {
                    currentNoteId = Convert.ToInt32(row["Id"]);
                }
                else
                {
                    // Handle the case where Id is null or invalid
                }
                Adding_Button.Visibility = Visibility.Collapsed;
                CardItemsControl.Visibility = Visibility.Collapsed;
                Writing_text_window.Visibility = Visibility.Visible;
            }
           
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            Button? deleteButton = sender as Button;
            DataRowView? row = deleteButton.DataContext as DataRowView;

            if (row != null)
            {
                if (row["Id"] != DBNull.Value)
                {
                    int noteId = Convert.ToInt32(row["Id"]);
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection("Data Source=DESKTOP-UR59NAB;Initial Catalog=NOTEAPP;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"))
                    {
                        con.Open();
                        string query = "DELETE FROM savenotes WHERE Id = @Id";
                        using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = noteId;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected note does not have a valid ID.", "Error", MessageBoxButton.OK);
                }
                LoadData();
            }
        }
        private void Note_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Grid? card = sender as Grid;
            DataRowView? row = card.DataContext as DataRowView;

            if (row != null)
            {
                string title = row["Titles"].ToString();
                string content = row["input"].ToString();

                // Do whatever you want — open or highlight
                MessageBox.Show($"Title: {title}\n\nContent: {content}", "Note Selected");
            }
        }

    }
}
 
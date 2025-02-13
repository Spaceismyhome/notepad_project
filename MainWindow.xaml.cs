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
using System.Data.SqlClient;
namespace notepad_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Add(object sender, RoutedEventArgs e)
        {
            Adding_Button.Visibility = Visibility.Collapsed;
            header.Visibility = Visibility.Collapsed;
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

        private void Save_button(object sender, EventArgs e)
        {
            //1-address of sql server and database
            string connectionstring = "Data Source=DESKTOP-UR59NAB;Initial Catalog=NotePad_project;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

            //2-establish connection
            SqlConnection con= new SqlConnection(connectionstring);

            //3-open connection

            //4-prepare query

            //5-excute query

            //6-close connection


        }
    }
 }
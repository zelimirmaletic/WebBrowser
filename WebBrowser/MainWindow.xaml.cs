using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;

namespace WebBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Uri currentUri;
        private Uri homePage = new Uri(getHomeAdress());
        public static bool isIncognito;

        public MainWindow()
        {
            InitializeComponent();
            webBrowser.Navigate(homePage);
            tbURL.Text = homePage.ToString();
            currentUri = homePage;
            tbIncognito.Text = "TRACKING";
            tbIncognito.Background = Brushes.Orange;
        }

        public Uri getCurrentUri() { return currentUri;  }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                webBrowser.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There are no more visited pages!");
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                webBrowser.GoForward();
                tbURL.Text = webBrowser.Source.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There are no more forward pages!");
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (tbURL.Text == "")
                MessageBox.Show("Enter adress or key words for search", "Warning");
            String googleURI = "https://www.google.com/search?&q=";
            if (tbURL.Text.StartsWith("https://") || tbURL.Text.StartsWith("http://"))
                webBrowser.Navigate(tbURL.Text);
            else
            {
                googleURI += tbURL.Text.Replace(" ", "+");
                webBrowser.Navigate(googleURI);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
                webBrowser.Source = new Uri(getHomeAdress());
                tbURL.Text=homePage.ToString();
        }

        private void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            tbURL.Text = e.Uri.OriginalString;
            currentUri = new Uri(e.Uri.OriginalString);
            //If connection is not secure mark that with background color
            if(e.Uri.OriginalString.StartsWith("https://"))
                tbURL.Background = new SolidColorBrush(Color.FromRgb(19,235,162));
            else
                tbURL.Background = new SolidColorBrush(Color.FromRgb(255, 136, 0));
            //IF (INCOGNITO IS NOT CHECKED)
            if(! isIncognito)
                writeToHistory(e.Uri.OriginalString);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(tbURL.Text);
        }

        private void btnStar_Click(object sender, RoutedEventArgs e)
        {
            string path = "AppData\\starred.txt";
            string[] urlForSaving = new string[1];
            urlForSaving[0] = webBrowser.Source.ToString();
            string[] lines = System.IO.File.ReadAllLines(@path);
            foreach (string line in lines)
            {
                if(line == urlForSaving[0])
                {
                    MessageBox.Show("This web page is already starred!");
                    return;
                }
            }
            //Write to a file
            System.IO.File.AppendAllLines(path, urlForSaving);
        }

        private void writeToHistory(string url)
        {
            string path = "AppData\\history.txt";
            string[] urlForSaving = new string[1];
            string dateTime = DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");
            urlForSaving[0] = dateTime + "  " + url;
            string[] lines = System.IO.File.ReadAllLines(@path);
            //Write to a file
            System.IO.File.AppendAllLines(path, urlForSaving);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.Topmost = true;
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
            
        }

        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
        }

        private static string getHomeAdress()
        {
            string path = "AppData\\home.txt";
            return System.IO.File.ReadAllText(path);
        }

        public string getHTML()
        {
            dynamic doc = webBrowser.Document;
            return  doc.documentElement.InnerHtml;

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.InvokeScript("execScript", new object[] { "window.print();", "JavaScript" });
        }
    }
}

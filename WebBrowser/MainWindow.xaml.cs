﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

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
                MessageBox.Show("Can't go back!","Warning", MessageBoxButton.OK,MessageBoxImage.Warning);
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
                MessageBox.Show("Can't go forward!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(tbURL.Text);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Source = new Uri(getHomeAdress());
            tbURL.Text = homePage.ToString();
        }

        public void btnGo_Click(object sender, RoutedEventArgs e)
        {
            //Make sure there is inout data
            if (tbURL.Text == "")
                MessageBox.Show("Enter adress or key words for search", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //Make Google format query
            String googleURI = "https://www.google.com/search?&q=";
            if (tbURL.Text.StartsWith("https://") || tbURL.Text.StartsWith("http://"))
                webBrowser.Navigate(tbURL.Text);
            else
            {
                googleURI += tbURL.Text.Replace(" ", "+");
                webBrowser.Navigate(googleURI);
            }
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

        private void btnStar_Click(object sender, RoutedEventArgs e)
        {
            string path = "AppData\\starred.txt";
            //Check if a page is already starred
            string[] urlForSaving = new string[1];
            //Generate current date
            string dateTime = DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");
            urlForSaving[0] = dateTime + "  " + webBrowser.Source.ToString();

            string[] lines = System.IO.File.ReadAllLines(@path);
            foreach (string line in lines)
            {
                if(line == urlForSaving[0])
                {
                    MessageBox.Show("This web page is already starred!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            //Write to a file
            System.IO.File.AppendAllLines(path, urlForSaving);
        }

        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.InvokeScript("execScript", new object[] { "window.print();", "JavaScript" });
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Show settings dialog
            Settings settingsWindow = new Settings();
            settingsWindow.Topmost = true;
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void writeToHistory(string url)
        {
            string path = "AppData\\history.txt";
            string[] urlForSaving = new string[1];
            //Generate current date
            string dateTime = DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");
            urlForSaving[0] = dateTime + "  " + url;
            string[] lines = System.IO.File.ReadAllLines(@path);
            //Write to a file
            System.IO.File.AppendAllLines(path, urlForSaving);
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

        public void setUrl(string url)
        {
            tbURL.Text = url;
        }


    }
}

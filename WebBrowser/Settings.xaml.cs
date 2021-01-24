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
using System.Windows.Shapes;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;


namespace WebBrowser
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private static bool flag = false;
        private static bool isWebAdress = false;
        public static void setFlag(bool state) { flag = state; }
        private string historyFilePath = "AppData\\history.txt";
        private string starredFilePath = "AppData\\starred.txt";

        public Settings()
        {
            InitializeComponent();
            this.cbIncognito.IsChecked = MainWindow.isIncognito;
        }

        private void btnShowHistory_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = true;
            //Clear all previous items
            lbShowArea.Items.Clear();
            //Set a new title
            tbAreaTitle.Text = "Your History";
            //Read all lines from the history.txt file
            string[] records = System.IO.File.ReadAllLines(historyFilePath);
            //Add all history lines to the list box
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            //If the history file is empty, show a message
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your history is empty!");
        }

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = false;
            //Clear all previous items
            clearShowArea();
            //If history file is already empty, show a message
            if (new System.IO.FileInfo(historyFilePath).Length == 0)
            {
                MessageBox.Show("Your history is already empty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //Make a new confirm window
            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.ShowDialog();
            //If answer is Yes, then delete history
            if (flag)
                System.IO.File.WriteAllText(historyFilePath,"");
        }

        private void btnShowStarred_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = true;
            //Clear all previous items
            lbShowArea.Items.Clear();
            //Set up a new title
            tbAreaTitle.Text = "Your Starred Pages";
            //Read all lines from the starred.txt file
            string[] records = System.IO.File.ReadAllLines(starredFilePath);
            //Add them to list box
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            //If the starred file is empty, show a message
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your don't have any starred pages!");
        }

        private void btnClearStarred_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = false;
            //Clear all previous items
            clearShowArea();
            //If starred file is already emty, show a message
            if (new System.IO.FileInfo(starredFilePath).Length == 0)
            {
                MessageBox.Show("Your starred pages list is already empty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //Make a new confirm box
            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.ShowDialog();
            //If answer is yes, delete starred pages
            if (flag)
                System.IO.File.WriteAllText(starredFilePath, "");
        }

        private void btnCert_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = false;
            //Clear all previous items
            clearShowArea();
            //Set a new title
            tbAreaTitle.Text = "Getting certificates. This may take a few moments...";
            //Do webrequest to get info on secure site
            Uri current = ((MainWindow)this.Owner).getCurrentUri();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(current.ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();
            //retrieve the ssl cert and assign it to an X509Certificate object
            X509Certificate cert = request.ServicePoint.Certificate;
            //If there is no certificate show a message box
            if (cert == null)
            {
                MessageBox.Show("This page has no SSL certificate!", "Info");
                return;
            }
            //convert the X509Certificate to an X509Certificate2 object by passing it into the constructor
            X509Certificate2 cert2 = new X509Certificate2(cert);
            //display the cert dialog box
            X509Certificate2UI.DisplayCertificate(cert2);
        }

        private void cbIncognito_Click(object sender, RoutedEventArgs e)
        {
            //Clear all previous items
            clearShowArea();
            //Set static variable
            MainWindow.isIncognito = (bool)cbIncognito.IsChecked;
            //Set incognito label 
            if (((MainWindow)this.Owner).tbIncognito.Text=="TRACKING")
            {
                tbAreaTitle.Text="You are now in Incognito mode, no history will be recorded!";
                ((MainWindow)this.Owner).tbIncognito.Text = "INCOGNITO";
                SolidColorBrush backgroundBrush = Brushes.MediumPurple;
                ((MainWindow)this.Owner).tbIncognito.Background=backgroundBrush;
            }
            else
            {
                tbAreaTitle.Text = "";
                ((MainWindow)this.Owner).tbIncognito.Text = "TRACKING";
                SolidColorBrush backgroundBrush = Brushes.Orange;
                ((MainWindow)this.Owner).tbIncognito.Background = backgroundBrush;
            }
        }

        private void btnSetHomePage_Click(object sender, RoutedEventArgs e)
        {
            //Clear all previous pages
            clearShowArea();
            ChangeHomePage newWindow = new ChangeHomePage();
            newWindow.Owner = this;
            newWindow.ShowDialog();
        }

        private void btnSourceCode_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = false;
            //Clear all previous items
            clearShowArea();
            if (lbShowArea.Items.IsEmpty)
            {
                //Set up a new title
                tbAreaTitle.Text = "Page source code";
                //Get document HTML code
                var htmlDocument = ((MainWindow)this.Owner).getHTML();
                //Split tags by <
                string[] tags = htmlDocument.Split('<');
                //Add all tags to item box
                foreach (var tag in tags)
                    lbShowArea.Items.Add("<" + tag);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Get page source code
            var htmlDocument = ((MainWindow)this.Owner).getHTML();
            //Show save dialog
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, htmlDocument);
                tbAreaTitle.Text = "Page saved to the local storage.";
            }
               
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            isWebAdress = false;
            //Clear all previous items
            clearShowArea();
            if (lbShowArea.Items.IsEmpty)
            {
                //Read about data and functionalities from txt file and display them to list box
                string[] records = System.IO.File.ReadAllLines("AppData\\about.txt");
                foreach (string data in records)
                    lbShowArea.Items.Add(data);
            }
        }

        private void clearShowArea()
        {
            tbAreaTitle.Text = "";
            lbShowArea.Items.Clear();
        }

        private void lbShowArea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(isWebAdress && !(lbShowArea.SelectedItem.ToString().StartsWith("Your")))
            {
                var split = lbShowArea.SelectedItem.ToString().Split(' ');
                string adress = split[4];
                ((MainWindow)this.Owner).setUrl(adress);
                ((MainWindow)this.Owner).btnGo_Click(sender, e);
            }
        }
    }
}

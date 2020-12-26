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
            lbShowArea.Items.Clear();
            tbAreaTitle.Text = "Your History";
            string[] records = System.IO.File.ReadAllLines(historyFilePath);
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your history is empty!");
        }

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();

            if (new System.IO.FileInfo(historyFilePath).Length == 0)
            {
                MessageBox.Show("Your history is already empty!", "Info");
                return;
            }
                

            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.ShowDialog();

            if (flag)
                System.IO.File.WriteAllText(historyFilePath,"");
        }

        private void btnShowStarred_Click(object sender, RoutedEventArgs e)
        {
            lbShowArea.Items.Clear();
            tbAreaTitle.Text = "Your Starred Pages";
            string[] records = System.IO.File.ReadAllLines(starredFilePath);
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your don't have any starred pages!");
        }

        private void btnClearStarred_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();

            if (new System.IO.FileInfo(starredFilePath).Length == 0)
            {
                MessageBox.Show("Your starred pages list is already empty!", "Info");
                return;
            }
                

            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.ShowDialog();

            if (flag)
                System.IO.File.WriteAllText(starredFilePath, "");
        }

        private void cbIncognito_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
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

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            if (lbShowArea.Items.IsEmpty)
            {
                tbAreaTitle.Text = "About";
                lbShowArea.Items.Add("T-Rex Web Browser");
                lbShowArea.Items.Add("Version 1.0");
                lbShowArea.Items.Add("Copyright Zelimir Maletic 2021.");
                lbShowArea.Items.Add("ETF Banja Luka");
                lbShowArea.Items.Add("");
                lbShowArea.Items.Add("FUNCTIONALITIES:");
                string[] records = System.IO.File.ReadAllLines("AppData\\functionalities.txt");
                foreach (string data in records)
                    lbShowArea.Items.Add(data);
            }
        }

        private void btnSetHomePage_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            ChangeHomePage newWindow = new ChangeHomePage();
            newWindow.ShowDialog();
        }

        private void btnSourceCode_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            if(lbShowArea.Items.IsEmpty)
            {
                tbAreaTitle.Text = "Page source code";
                var htmlDocument = ((MainWindow)this.Owner).getHTML();

                string[] tags = htmlDocument.Split('<');

                foreach (var tag in tags)
                    lbShowArea.Items.Add("<"+tag);


            }
           
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var htmlDocument = ((MainWindow)this.Owner).getHTML();
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, htmlDocument);
        }

        private void clearShowArea()
        {
            tbAreaTitle.Text = "";
            lbShowArea.Items.Clear();
        }       

        private void btnCert_Click(object sender, RoutedEventArgs e)
        {
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
    }
}

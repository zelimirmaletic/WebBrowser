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
using System.Windows.Media;
using System.Windows.Controls;

namespace WebBrowser
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private static bool flag = false;
        public static void setFlag(bool state) { flag = state; }
        public Settings()
        {
            InitializeComponent();
            this.cbIncognito.IsChecked = MainWindow.isIncognito;
            
        }

        private void btnShowHistory_Click(object sender, RoutedEventArgs e)
        {
            lbShowArea.Items.Clear();
            tbAreaTitle.Text = "Your History";
            string path = "AppData\\history.txt";
            string[] records = System.IO.File.ReadAllLines(path);
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your history is empty!");
        }

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.Show();

            if (flag)
            {
                string path = "AppData\\history.txt";
                System.IO.File.WriteAllText(path,"");
            }
        }

        private void btnShowStarred_Click(object sender, RoutedEventArgs e)
        {
            lbShowArea.Items.Clear();
            tbAreaTitle.Text = "Your Starred Pages";
            string path = "AppData\\starred.txt";
            string[] records = System.IO.File.ReadAllLines(path);
            foreach (string data in records)
                lbShowArea.Items.Add(data);
            if (lbShowArea.Items.IsEmpty)
                lbShowArea.Items.Add("Your don't have any starred pages!");
        }

        private void btnClearStarred_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            ConfirmBox confirmBox = new ConfirmBox();
            confirmBox.Topmost = true;
            confirmBox.Show();

            if (flag)
            {
                string path = "AppData\\starred.txt";
                System.IO.File.WriteAllText(path, "");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
           
            // Create the print dialog object and set options
           // PrintDialog printDialog = new PrintDialog();
           // printDialog.PrintDocument(((IDocumentPaginatorSource)((MainWindow)this.Owner).webBrowser.Document).DocumentPaginator, "My App");  
        }

        private void cbIncognito_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            MainWindow.isIncognito = (bool)cbIncognito.IsChecked;
            //Set incognito label 
            var mainWindow = ((MainWindow)this.Owner);
            if (mainWindow.tbIncognito.Text=="TRACKING")
            {
                mainWindow.tbIncognito.Text = "INCOGNITO";
                SolidColorBrush backgroundBrush = Brushes.MediumPurple;
                mainWindow.tbIncognito.Background=backgroundBrush;
            }
            else
            {
                mainWindow.tbIncognito.Text = "TRACKING";
                SolidColorBrush backgroundBrush = Brushes.Orange;
                mainWindow.tbIncognito.Background = backgroundBrush;
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            tbAreaTitle.Text = "About";
            lbShowArea.Items.Add("T-Rex Web Browser");
            lbShowArea.Items.Add("Version 1.0");
            lbShowArea.Items.Add("Copyright Zelimir Maletic 2021.");
            lbShowArea.Items.Add("ETF Banja Luka");

        }

        private void btnSetHomePage_Click(object sender, RoutedEventArgs e)
        {
            clearShowArea();
            ChangeHomePage newWindow = new ChangeHomePage();
            newWindow.Show();
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
    }
}

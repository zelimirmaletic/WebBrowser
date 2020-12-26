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

namespace WebBrowser
{
    /// <summary>
    /// Interaction logic for ChangeHomePage.xaml
    /// </summary>
    public partial class ChangeHomePage : Window
    {
        public ChangeHomePage()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbHomeAdress.Text == "")
                MessageBox.Show("Adress is empty, plase enter one.");
            else
            {
                string path = "AppData\\home.txt";
                System.IO.File.WriteAllText(path,"https://" + tbHomeAdress.Text.ToLower());
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

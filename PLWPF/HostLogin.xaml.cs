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
using Utilities;
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Host.xaml
    /// </summary>
    public partial class HostLogin : Window
    {
        IBL bL = BlFactory.getBl();

        public HostLogin()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new HostForm().Show();
            Owner.Close();
            Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!Tools.ValidateNumber(KeyTextBox.Text))
            {
                MessageBox.Show("תעודת זהות לא תקינה");
                return;
            }

            Host host = bL.GetHost(int.Parse(KeyTextBox.Text));
            if ( host== null)
            {
                MessageBox.Show("תעודת זהות לא קיימת במאגר.");
                return;
            }

            new PrivateZone(host).Show();
            Owner.Close();
            Close();
        }
    }
}

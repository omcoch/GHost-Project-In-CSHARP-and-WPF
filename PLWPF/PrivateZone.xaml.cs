using BE;
using BL;
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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for PrivateZone.xaml
    /// </summary>
    public partial class PrivateZone : Window
    {
        IBL bL = BlFactory.getBl();
        Host host;

        public PrivateZone()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
            this.host=bL.GetHost(Cookies.LoginUserKey);
            WelcomeMsg.Text = "ברוך הבא " + host.PrivateName + " " + host.FamilyName + "!";
        }

        private void OpenHostingUnitForm(object sender, RoutedEventArgs e)
        { 
            new HostingUnitForm().Show();
            Close();
        }

        private void HostingUnitsList_Click(object sender, RoutedEventArgs e)
        {
            new HostingUnitsList().Show();
            Close();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
            Close();
        }
    }
}

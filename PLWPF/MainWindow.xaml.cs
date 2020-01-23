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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bL = BlFactory.getBl();

        public MainWindow()
        {
            Cookies.LastWindow = this;
            Cookies.LoginUserKey = 0;
            InitializeComponent();
        }

        private void OpenGuestRequestWindow(object sender, RoutedEventArgs e)
        {
            new GuestRequest().Show();
            Close();
        }

        private void OpenHostWindow(object sender, RoutedEventArgs e)
        {
            new HostLogin(){ Owner = this }.ShowDialog();            
            //new HostLogin(){ Owner = this }.Show();            
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cookies.LastWindow = this;           
        }

    }
}

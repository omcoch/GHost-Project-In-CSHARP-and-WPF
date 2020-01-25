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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
            if (Window.NameProperty.Name == "MainWindow")
                ReturnButton.Visibility = Visibility.Hidden;
        }

        //private void GoBackWindow(object sender, RoutedEventArgs e)
        //{
        //    Window.GetWindow(Cookies.LastWindow).Show();
        //    Window.GetWindow(this).Close();
        //}

        private void GoHome(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Window.GetWindow(this).Close();
        }
    }
}

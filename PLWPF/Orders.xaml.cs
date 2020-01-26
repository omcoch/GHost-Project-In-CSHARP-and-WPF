using BE;
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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            Cookies.PrevWindow = this.GetType().Name;

            InitializeComponent();

        }

        private void OpenGuestRequestList(object sender, RoutedEventArgs e)
        {
            new GuestsRequestList().Show();
            Close();
        }

        private void OpenOrderList(object sender, RoutedEventArgs e)
        {
            new OrdersList().Show();
            Close();
        }
    }
}

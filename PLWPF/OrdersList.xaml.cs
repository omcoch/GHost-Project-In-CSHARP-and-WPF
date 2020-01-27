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
    /// Interaction logic for OrdersList.xaml
    /// </summary>
    public partial class OrdersList : Window
    {
        IBL bL = BlFactory.getBl();


        public OrdersList()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            orderViewSource.Source = bL.GetOrdersByHostKey(Cookies.LoginUserKey);
        }

        private void UpdateOrders(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Order order in orderDataGrid.SelectedItems)
                {                    
                    bL.UpdateOrder(order);
                }
                MessageBox.Show("ההזמנות עודכנו בהצלחה!");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(orderDataGrid.SelectedItem!=null)
                ((Order)orderDataGrid.SelectedItem).Status = (OrderStatus)e.AddedItems[0];
        }
    }
}

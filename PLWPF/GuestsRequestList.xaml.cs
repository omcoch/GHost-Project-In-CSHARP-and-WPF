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
    /// Interaction logic for GuestsRequestList.xaml
    /// </summary>
    public partial class GuestsRequestList : Window
    {
        IBL bL = BlFactory.getBl();

        public GuestsRequestList()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            guestRequestViewSource.Source = bL.GetGuestRequestsByCondition(x => true);
            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            hostingUnitViewSource.Source = bL.GetHostingUnitsByOwner(Cookies.LoginUserKey);
        }

        private void SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count != 0)
                CreateButton.IsEnabled = true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            BE.GuestRequest gr = (BE.GuestRequest)guestRequestDataGrid.SelectedItem;
            HostingUnit hu = (HostingUnit)hostingUnitComboBox.SelectedItem;
            Order order = new Order() {
                GuestRequestKey = gr.guestRequestKey,
                CreateDate = DateTime.Now,
                HostingUnitKey=hu.HostingUnitKey,
                Status= OrderStatus.טרם_טופל
            };
            try
            {
                int key = bL.AddOrder(order);
                MessageBox.Show("ההזמנה נוצרה בהצלחה", "מספר הזמנה " + key);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}

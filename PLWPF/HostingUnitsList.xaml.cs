using BL;
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
    /// Interaction logic for HostingUnitsList.xaml
    /// </summary>
    public partial class HostingUnitsList : Window
    {
        IBL bL = BlFactory.getBl();

        public HostingUnitsList()
        {
            Cookies.PrevWindow = this.GetType().Name;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            hostingUnitViewSource.Source = bL.GetHostingUnitsByOwner(Cookies.LoginUserKey);
        }

        private void DeleteUnits(object sender, RoutedEventArgs e)
        {
            try
            {
                HostingUnit hu = (HostingUnit)hostingUnitDataGrid.SelectedItem;
                bL.RemoveHostingUnit(hu.HostingUnitKey);
                MessageBox.Show("יחידת האירוח נמחקה בהצלחה!");
                new PrivateZone().Show();
                Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);                
            }
        }

        private void UpdateUnits(object sender, RoutedEventArgs e)
        {
            HostingUnit hu = (HostingUnit)hostingUnitDataGrid.SelectedItem;
            new HostingUnitForm(hu).Show();
            Close();
        }

        private void HostingUnitDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            update.IsEnabled = true;
            remove.IsEnabled = true;
        }
    }
}

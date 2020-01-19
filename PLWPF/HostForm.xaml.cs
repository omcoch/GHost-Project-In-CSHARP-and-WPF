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
    /// Interaction logic for HostForm.xaml
    /// </summary>
    public partial class HostForm : Window
    {
        public HostForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource bankBranchViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("bankBranchViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // bankBranchViewSource.Source = [generic data source]
        }
    }
}

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
    /// Interaction logic for HostForm.xaml
    /// </summary>
    public partial class HostForm : Window
    {
        IBL bL = BlFactory.getBl();
        Host host;
        
        public HostForm(int hostKey = 0)
        {
            InitializeComponent();

            host= bL.GetHost(hostKey);
            grid1.DataContext = host;
            if(hostKey==0)
                foreach (var g in grid1.Children)
                {
                    g.IsEnabled = false;
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
        }
    }
}

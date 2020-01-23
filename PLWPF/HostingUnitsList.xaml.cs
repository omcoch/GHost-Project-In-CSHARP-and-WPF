﻿using BL;
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
        int id;

        public HostingUnitsList(int key)
        {
            InitializeComponent();
            id = key;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            hostingUnitViewSource.Source = bL.GetHostingUnitsByOwner(id);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cookies.LastWindow = Window.NameProperty.Name;
        }
    }
}

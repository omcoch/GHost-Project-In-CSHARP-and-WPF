﻿using System;
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
            Cookies.PrevWindow = this.GetType().Name;

            InitializeComponent();

            Header.HomeButton.Visibility = Visibility.Hidden;
        }

        private void OpenGuestRequestWindow(object sender, RoutedEventArgs e)
        {
            new GuestRequest().Show();
            Close();
        }

        private void OpenHostWindow(object sender, RoutedEventArgs e)
        {
            new HostLogin(){ Owner = this }.ShowDialog();                  
        }
        private void OpenAdmin(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
            Close();
        }

    }
}

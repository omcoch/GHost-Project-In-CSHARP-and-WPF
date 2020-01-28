﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PLWPF.UserControls
{
    /// <summary>
    /// Interaction logic for HostView.xaml
    /// </summary>
    public partial class HostView : UserControl
    {
        public ObservableCollection<BE.Host> Collection = new ObservableCollection<BE.Host>();
        public HostView()
        {
            InitializeComponent();
            DataContext = Collection;
        }
    }
}

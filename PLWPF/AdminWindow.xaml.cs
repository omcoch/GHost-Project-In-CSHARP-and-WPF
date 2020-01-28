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
    public enum ChoiceList {GuestRequest,Host, HostingUnit, Order}


    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        IBL bL = BlFactory.getBl();
        
        List<string> comboBoxListGr = new List<string>() {"קבץ לפי אזור" , "קבץ לפי מספר נופשים"}; //רשימת שאילתות דרישות לקוח
        List<string> comboBoxListHu = new List<string>() { "קבץ לפי אזור" }; // רשימת שאילתות יחידות אירוח
        List<string> comboBoxListH = new List<string>() { "קבץ לפי מספר יחידות אירוח"}; // רשימת שאילתות מארחים
        List<string> comboBoxListO = new List<string>() { "סנן לפי","מיין לפי"}; // רשימת שאילתות הזמנות
        

        public AdminWindow()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           switch((ChoiceList)ListComboBox.SelectedIndex)
            {
                case ChoiceList.GuestRequest:
                    QueryComboBox.ItemsSource = comboBoxListGr;
                    break;
                case ChoiceList.Host:
                    QueryComboBox.ItemsSource = comboBoxListH;
                    break;
                case ChoiceList.HostingUnit:
                    QueryComboBox.ItemsSource = comboBoxListHu;
                    break;
                case ChoiceList.Order:
                    QueryComboBox.ItemsSource = comboBoxListO;
                    break;
            }
            QueryComboBox.SelectedIndex = 0;
        }

        private void QueryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QueryComboBox.SelectedItem != null)
                if (QueryComboBox.SelectedItem.ToString() == "מיין לפי" || QueryComboBox.SelectedItem.ToString() == "סנן לפי")
                {
                    ConditionComboBox.Visibility = Visibility.Visible;
                    ConditionComboBox.ItemsSource = from h in typeof(Order).GetProperties() select h.Name;
                }
                else
                    ConditionComboBox.Visibility = Visibility.Hidden;
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            switch ((ChoiceList)ListComboBox.SelectedIndex)
            {
                case ChoiceList.GuestRequest:
                    if (QueryComboBox.SelectedItem.ToString() == "קבץ לפי אזור")
                    {
                        var v=new UserControls.GuestRequestView();
                        
                        foreach (var item in bL.GetGuestRequestsGroupByArea())
                        {
                            foreach (var gu in item)
                            {
                                v.Collection.Add(gu);
                            } 
                        }
                        ContentControl.Content = v;
                    }
                    else
                        bL.GetGuestRequestsGroupByVacationersNumber();
                    break;
                case ChoiceList.Host:
                    break;
                case ChoiceList.HostingUnit:
                    break;
                case ChoiceList.Order:
                    break;
                default:
                    break;
            }
        }
    }
}

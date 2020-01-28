using BE;
using BL;
using System;
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
using System.Windows.Shapes;

namespace PLWPF
{
    public enum ChoiceList { GuestRequest, Host, HostingUnit, Order }


    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        IBL bL = BlFactory.getBl();

        List<string> comboBoxListGr = new List<string>() { "קבץ לפי אזור", "קבץ לפי מספר נופשים" }; //רשימת שאילתות דרישות לקוח
        List<string> comboBoxListHu = new List<string>() { "קבץ לפי אזור" }; // רשימת שאילתות יחידות אירוח
        List<string> comboBoxListH = new List<string>() { "קבץ לפי מספר יחידות אירוח" }; // רשימת שאילתות מארחים
        List<string> comboBoxListO = new List<string>() { "סנן לפי", "מיין לפי" }; // רשימת שאילתות הזמנות
        private ObservableCollection<UserControl> UserControlCollection = new ObservableCollection<UserControl>();

        public AdminWindow()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
            DataContext = UserControlCollection;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch ((ChoiceList)ListComboBox.SelectedIndex)
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
            {
                if (QueryComboBox.SelectedItem.ToString() == "מיין לפי")
                {
                    ConditionComboBox.Visibility = Visibility.Visible;
                    ConditionComboBox.ItemsSource = from h in typeof(Order).GetProperties() select h.Name;
                    ConditionComboBox.SelectedIndex = 0;
                }
                else if (QueryComboBox.SelectedItem.ToString() == "סנן לפי")
                {
                    ConditionComboBox.Visibility = Visibility.Visible;
                    ConditionComboBox.ItemsSource = new List<string>() { "הזמנות פתוחות", "הזמנות סגורות", "הזמנות שנסגרו בהיענות לקוח" };
                    ConditionComboBox.SelectedIndex = 0;
                }
                else
                    ConditionComboBox.Visibility = Visibility.Hidden;
            }
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            UserControlCollection.Clear();
            switch ((ChoiceList)ListComboBox.SelectedIndex)
            {
                case ChoiceList.GuestRequest:
                    if (QueryComboBox.SelectedItem.ToString() == "קבץ לפי אזור")
                    {
                        foreach (var item in bL.GetGuestRequestsGroupByArea())
                        {
                            var uc = new UserControls.GuestRequestView();
                            uc.group.Header = item.Key.ToString();

                            foreach (var gu in item)
                            {
                                uc.Collection.Add(gu);
                            }
                            UserControlCollection.Add(uc);
                        }
                        listBox.ItemsSource = UserControlCollection;
                    }
                    else
                    {
                        foreach (var item in bL.GetGuestRequestsGroupByVacationersNumber())
                        {
                            var uc = new UserControls.GuestRequestView();
                            uc.group.Header = item.Key.ToString();

                            foreach (var gu in item)
                            {
                                uc.Collection.Add(gu);
                            }
                            UserControlCollection.Add(uc);
                        }
                        listBox.ItemsSource = UserControlCollection;
                    }
                    break;
                case ChoiceList.Host:
                    foreach (var item in bL.GetHostsGroupByNumOfUnits())
                    {
                        var uc = new UserControls.HostView();
                        uc.group.Header = item.Key.ToString();

                        foreach (var h in item)
                        {
                            uc.Collection.Add(h);
                        }
                        UserControlCollection.Add(uc);
                    }
                    listBox.ItemsSource = UserControlCollection;
                    break;
                case ChoiceList.HostingUnit:
                    foreach (var item in bL.GetHostingUnitsGroupByArea())
                    {
                        var uc = new UserControls.HostingUnitView();
                        uc.group.Header = item.Key.ToString();

                        foreach (var hu in item)
                        {
                            uc.Collection.Add(hu);
                        }
                        UserControlCollection.Add(uc);
                    }
                    listBox.ItemsSource = UserControlCollection;
                    break;
                case ChoiceList.Order:
                    List<Order> orders;
                    if (QueryComboBox.SelectedItem.ToString() == "מיין לפי")
                    {
                        if (ConditionComboBox.SelectedItem.ToString() == "HostingUnitKey")
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.HostingUnitKey
                                      select item).ToList();
                        }
                        else if (ConditionComboBox.SelectedItem.ToString() == "GuestRequestKey")
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.GuestRequestKey
                                      select item).ToList();
                        }
                        else if (ConditionComboBox.SelectedItem.ToString() == "OrderKey")
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.OrderKey
                                      select item).ToList();
                        }
                        else if (ConditionComboBox.SelectedItem.ToString() == "Status")
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.Status
                                      select item).ToList();
                        }
                        else if (ConditionComboBox.SelectedItem.ToString() == "CreateDate")
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.CreateDate
                                      select item).ToList();
                        }
                        else
                        {
                            orders = (from item in bL.GetOrdersByCondition(k => true)
                                      orderby item.OrderDate
                                      select item).ToList();
                        }
                    }
                    else
                    {
                        if (ConditionComboBox.SelectedItem.ToString() == "הזמנות פתוחות")
                        {
                            orders = bL.GetOrdersByCondition(o => o.Status == OrderStatus.טרם_טופל || o.Status == OrderStatus.נשלח_מייל);
                        }
                        else if (ConditionComboBox.SelectedItem.ToString() == "הזמנות סגורות")
                        {
                            orders = bL.GetOrdersByCondition(o => o.Status == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח || o.Status == OrderStatus.נסגר_בעקבות_סגירת_עסקה_עם_מארח_אחר || o.Status == OrderStatus.נסגר_בהיענות_של_הלקוח);
                        }
                        else
                        {
                            orders = bL.GetOrdersByCondition(o => o.Status == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח);
                        }
                    }
                    var uC = new UserControls.OrderView();
                    if (orders.Any())
                    {
                        foreach (var o in orders)
                        {
                            //uc.group.Header = item.Key.ToString();
                            uC.Collection.Add(o);
                        }
                        UserControlCollection.Add(uC);
                        listBox.ItemsSource = UserControlCollection;
                    }
                    else
                        MessageBox.Show("אין נתונים להצגה");
                    
                    break;
                default:
                    break;
            }
        }
    }
}

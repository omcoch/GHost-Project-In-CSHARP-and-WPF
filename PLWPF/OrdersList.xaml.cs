using BE;
using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
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
using Utilities;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrdersList.xaml
    /// </summary>
    public partial class OrdersList : Window
    {
        IBL bL = BlFactory.getBl();
        BackgroundWorker worker;

        public OrdersList()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();

            worker = new BackgroundWorker();
            worker.DoWork += Worker_EmailSend;
            worker.RunWorkerCompleted += Worker_EmailSent;
        }

        private void Worker_EmailSent(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
                MessageBox.Show("אירעה תקלה בשליחת המייל לכתובת " + e.Result, "שגיאה");
            else
                MessageBox.Show("נשלח מייל בהצלחה לכתובת " + e.Result);
        }

        private void Worker_EmailSend(object sender, DoWorkEventArgs e)
        {
            e.Result = ((MailMessage)e.Argument).To;
            bool flag = false;
            while (!flag)
            {
                try
                {
                    flag=Tools.SendMail((MailMessage)e.Argument, Configuration.SiteName, Configuration.AdminMailAddress.Address);
                }
                catch
                {
                    
                }
                System.Threading.Thread.Sleep(2000);
            }
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            orderViewSource.Source = bL.GetOrdersByHostKey(Cookies.LoginUserKey);
        }

        private void UpdateOrders(object sender, RoutedEventArgs e)
        {
            MailMessage message=new MailMessage();
            try
            {
                foreach (Order order in orderDataGrid.SelectedItems)
                { 
                    bL.UpdateOrder(order,ref message);
                    if (order.Status==OrderStatus.נשלח_מייל)
                        worker.RunWorkerAsync(message);
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

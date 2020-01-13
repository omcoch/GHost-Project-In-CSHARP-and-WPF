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
    /// Interaction logic for GuestRequest.xaml
    /// </summary>
    public partial class GuestRequest : Window
    {
        public GuestRequest()
        {
            InitializeComponent();
            PrivateNameMessage.Visibility = Visibility.Hidden;
            FamilyNameMessage.Visibility = Visibility.Hidden;
            EmailMessage.Visibility = Visibility.Hidden;
            NumbersMessage.Visibility = Visibility.Hidden;
            SubAreaMessage.Visibility = Visibility.Hidden;
        }

        private void Send_Request(object sender, RoutedEventArgs e)
        {
            
            BE.GuestRequest newGuestRequest = new BE.GuestRequest()
            {
                PrivateName = PrivateName.Text,
                FamilyName = FamilyName.Text,
                //MailAddress = Email.
            };
            
        }

        private void PrivateName_KeyUp(object sender, KeyEventArgs e)
        {
            if (!BE.Tools.validateString(PrivateName.Text))
                PrivateNameMessage.Visibility = Visibility.Visible;
            else
                PrivateNameMessage.Visibility = Visibility.Hidden;
        }

        private void ChildrenNum_KeyDown(object sender, KeyEventArgs e)
        {
            int key = Convert.ToUInt16(e.Key);
            
        }
    }
}

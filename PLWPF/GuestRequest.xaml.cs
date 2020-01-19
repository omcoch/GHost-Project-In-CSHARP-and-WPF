using BE;
using BL;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for GuestRequest.xaml
    /// </summary>
    public partial class GuestRequest : Window
    {
        IBL bL = BlFactory.getBl();


        public GuestRequest()
        {
            InitializeComponent();

            PrivateNameMessage.Visibility = Visibility.Hidden;
            FamilyNameMessage.Visibility = Visibility.Hidden;
            EmailMessage.Visibility = Visibility.Hidden;
            NumbersMessage.Visibility = Visibility.Hidden;

            Area.ItemsSource = Enum.GetValues(typeof(Regions));
            Area.SelectedIndex = 1;
            Type.ItemsSource = Enum.GetValues(typeof(GRType));
            Type.SelectedIndex = 0;
            Price.ItemsSource = Enum.GetValues(typeof(MaxPrice));
            Price.SelectedIndex = 3;
            Pool.ItemsSource = Enum.GetValues(typeof(Requirements));
            Pool.SelectedIndex = 1;
            Hottub.ItemsSource = Enum.GetValues(typeof(Requirements));
            Hottub.SelectedIndex = 1;
            Garden.ItemsSource = Enum.GetValues(typeof(Requirements));
            Garden.SelectedIndex = 1;
            Atractions.ItemsSource = Enum.GetValues(typeof(Requirements));
            Atractions.SelectedIndex = 1;
        }

        private void Send_Request(object sender, RoutedEventArgs e)
        {
            DateTime? entryDate = Calendar.GetEntryDate();
            if (entryDate == null
                || !Tools.validateString(PrivateName.Text) || !Tools.validateString(FamilyName.Text)
                || !Tools.ValidateEmailAddress(Email.Text)
                || (int.Parse(ChildrenNum.Text)<=0 && int.Parse(AdultsNum.Text) <= 0))
            {
                MessageBox.Show("לא כל השדות מולאו", "שגיאה");
                return;
            }

            BE.GuestRequest newGuestRequest = new BE.GuestRequest()
            {
                PrivateName = PrivateName.Text,
                FamilyName = FamilyName.Text,
                MailAddress = new MailAddress(Email.Text),
                Area = (Regions)Enum.Parse(typeof(Regions), Area.SelectedValue.ToString()),
                SubArea = SubArea.Text,
                Type = (GRType)Enum.Parse(typeof(GRType), Type.SelectedValue.ToString()),
                Adults = int.Parse(AdultsNum.Text),
                Children = int.Parse(ChildrenNum.Text),
                MaxPrice= (MaxPrice)Enum.Parse(typeof(MaxPrice), Price.SelectedValue.ToString()),
                Pool= (Requirements)Enum.Parse(typeof(Requirements), Pool.SelectedValue.ToString()),
                Jacuzzi = (Requirements)Enum.Parse(typeof(Requirements), Hottub.SelectedValue.ToString()),
                Garden = (Requirements)Enum.Parse(typeof(Requirements), Garden.SelectedValue.ToString()),
                ChildrensAttractions = (Requirements)Enum.Parse(typeof(Requirements), Atractions.SelectedValue.ToString()),
                RegistrationDate=DateTime.Now,
                EntryDate = (DateTime)entryDate,
                ReleaseDate = (DateTime)Calendar.GetReleaseDate(),
                Status = RequestStatus.פתוחה,                
            };

            try
            {
                int code = bL.AddGuestRequest(newGuestRequest);
                MessageBox.Show("הבקשה נקלטה במערכת, תודה רבה!", "בקשה מספר "+code);
                new MainWindow().Show();
                Close();
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show(err.ParamName, "שגיאה");
            }
            catch
            {
                MessageBox.Show("שגיאה לא ידועה. אנא פנה למנהל המערכת.", "שגיאה");
            }
        }

        private void PrivateName_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Tools.validateString(PrivateName.Text))
                PrivateNameMessage.Visibility = Visibility.Visible;
            else
                PrivateNameMessage.Visibility = Visibility.Hidden;
        }

        private void NumbersTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            if (!Tools.ValidateNumber(ChildrenNum.Text) || !Tools.ValidateNumber(AdultsNum.Text))
                NumbersMessage.Visibility = Visibility.Visible;
            else
                NumbersMessage.Visibility = Visibility.Hidden;
        }

        private void FamilyName_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Tools.validateString(FamilyName.Text))
                FamilyNameMessage.Visibility = Visibility.Visible;
            else
                FamilyNameMessage.Visibility = Visibility.Hidden;
        }

        private void Email_KeyUp(object sender, KeyEventArgs e)
        {

            if (!Tools.ValidateEmailAddress(Email.Text))
                EmailMessage.Visibility = Visibility.Visible;
            else
                EmailMessage.Visibility = Visibility.Hidden;
        }
        
    }
}

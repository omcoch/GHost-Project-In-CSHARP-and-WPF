﻿using BE;
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
        BE.GuestRequest guest = new BE.GuestRequest();

        public GuestRequest()
        {
            Cookies.PrevWindow = this.GetType().Name;

            InitializeComponent();
            guest.EntryDate = DateTime.Now.Date;
            guest.ReleaseDate = DateTime.Now.Date;

            GuestRequestGrid.DataContext = guest;

            PrivateNameMessage.Visibility = Visibility.Hidden;
            FamilyNameMessage.Visibility = Visibility.Hidden;
            EmailMessage.Visibility = Visibility.Hidden;
            NumbersMessage.Visibility = Visibility.Hidden;
            DatesMessage.Visibility = Visibility.Hidden;

            areaComboBox.ItemsSource = Enum.GetValues(typeof(Regions));
            areaComboBox.SelectedIndex = 1;
            typeComboBox.ItemsSource = Enum.GetValues(typeof(GRType));
            typeComboBox.SelectedIndex = 0;
            poolComboBox.ItemsSource = Enum.GetValues(typeof(Requirements));
            poolComboBox.SelectedIndex = 1;
            jacuzziComboBox.ItemsSource = Enum.GetValues(typeof(Requirements));
            jacuzziComboBox.SelectedIndex = 1;
            gardenComboBox.ItemsSource = Enum.GetValues(typeof(Requirements));
            gardenComboBox.SelectedIndex = 1;
            childrensAttractionsComboBox.ItemsSource = Enum.GetValues(typeof(Requirements));
            childrensAttractionsComboBox.SelectedIndex = 1;            
        }

        private void Send_Request(object sender, RoutedEventArgs e)
        {
            if (!Tools.ValidateString(privateNameTextBox.Text) || !Tools.ValidateString(familyNameTextBox.Text)
                || !Tools.ValidateEmailAddress(mailAddressTextBox.Text)
                || (int.Parse(childrenTextBox.Text) <= 0 && int.Parse(adultsTextBox.Text) <= 0))
            {
                MessageBox.Show("לא כל השדות מולאו", "שגיאה");
                return;
            }

            try
            {
                guest.MailAddress = new MailAddress(mailAddressTextBox.Text);
                guest.RegistrationDate = DateTime.Now.Date;
                guest.Status = RequestStatus.פתוחה;
              
                int code = bL.AddGuestRequest(guest);
                MessageBox.Show("הבקשה נקלטה במערכת, תודה רבה!", "בקשה מספר " + code);
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
            if (!Tools.ValidateString(privateNameTextBox.Text))
                PrivateNameMessage.Visibility = Visibility.Visible;
            else
                PrivateNameMessage.Visibility = Visibility.Hidden;
        }

        private void NumbersTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            if (!Tools.ValidateNumber(childrenTextBox.Text, 99) || !Tools.ValidateNumber(adultsTextBox.Text, 99))
                NumbersMessage.Visibility = Visibility.Visible;
            else
                NumbersMessage.Visibility = Visibility.Hidden;
        }

        private void FamilyName_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Tools.ValidateString(familyNameTextBox.Text))
                FamilyNameMessage.Visibility = Visibility.Visible;
            else
                FamilyNameMessage.Visibility = Visibility.Hidden;
        }

        private void Email_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Tools.ValidateEmailAddress(mailAddressTextBox.Text))
                EmailMessage.Visibility = Visibility.Visible;
            else
                EmailMessage.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// בודק האם טווח התאריכים שהוזן תקין בכל שינוי של השדות.    
        /// אחראי על הופעת/הסתרת השגיאה.
        /// <para>
        /// בדיקה 1: תאריך יציאה לפחות יום אחד אחרי תאריך כניסה.
        /// בדיקה 2: תאריך כניסה לא עבר כבר.
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_CalendarClosed(object sender, RoutedEventArgs e)
        {
            TimeSpan timeSpan1 = releaseDateDatePicker.SelectedDate.Value - entryDateDatePicker.SelectedDate.Value,
                timeSpan2 = entryDateDatePicker.SelectedDate.Value - DateTime.Now.Date;
            if ( timeSpan1.TotalDays < 1 || timeSpan2.TotalDays < 0)
                DatesMessage.Visibility = Visibility.Visible;
            else
                DatesMessage.Visibility = Visibility.Hidden;
        }

    }
}
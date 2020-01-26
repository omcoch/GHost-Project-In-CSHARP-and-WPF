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
    /// Interaction logic for HostForm.xaml
    /// </summary>
    public partial class HostForm : Window
    {
        IBL bL = BlFactory.getBl();
        Host host; 
        
        /// <summary>
        /// קונסטרקטור ליצירת מארח חדש
        /// </summary>
        public HostForm()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();

            submitButton.Content = "הוסף";
            submitButton.Click += AddHost;
            host = new Host();// get default Host if does not exists
            grid1.DataContext = host; /// data binding the host to the visual form 
        }

        /// <summary>
        /// קונסטרקטור לעדכון מארח
        /// </summary>
        /// <param name="hostKey">מזהה מארח</param>
        public HostForm(int hostKey)
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();

            submitButton.Click += UpdateHost;
            host = bL.GetHost(hostKey); // get default Host if does not exists
            grid1.DataContext = host; /// data binding the host to the visual form 
            hostKeyTextBox.IsEnabled = false;            
        }

        private void AddHost(object sender, RoutedEventArgs e)
        {
            if (!Tools.ValidateString(privateNameTextBox.Text) || !Tools.ValidateString(familyNameTextBox.Text)
                || !Tools.ValidatePhoneNumber(phoneNumberTextBox.Text) || !Tools.ValidateNumber(hostKeyTextBox.Text)
                || !Tools.ValidateEmailAddress(addressTextBox.Text) || !Tools.ValidateNumber(bankNumberTextBox.Text)
                || !Tools.ValidateString(bankNameTextBox.Text) || !Tools.ValidateNumber(bankAccountNumberTextBox.Text)
                || !Tools.ValidateNumber(branchNumberTextBox.Text) || string.IsNullOrEmpty(branchAddressTextBox.Text)
                || !Tools.ValidateString(branchCityTextBox.Text)
                )
                MessageBox.Show("לא כל השדות מולאו כראוי");
            else
                try
                {
                    bL.AddHost(host);
                    MessageBox.Show("ההרשמה בוצעה בהצלחה");
                    new MainWindow().Show();
                    Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
        }
        private void UpdateHost(object sender, RoutedEventArgs e)
        {
            if (!Tools.ValidateString(privateNameTextBox.Text) || !Tools.ValidateString(familyNameTextBox.Text)
                || !Tools.ValidatePhoneNumber(phoneNumberTextBox.Text) || !Tools.ValidateNumber(hostKeyTextBox.Text)
                || !Tools.ValidateEmailAddress(addressTextBox.Text) || !Tools.ValidateNumber(bankNumberTextBox.Text)
                || !Tools.ValidateString(bankNameTextBox.Text) || !Tools.ValidateNumber(bankAccountNumberTextBox.Text)
                || !Tools.ValidateNumber(branchNumberTextBox.Text) || string.IsNullOrEmpty(branchAddressTextBox.Text)
                || !Tools.ValidateString(branchCityTextBox.Text))
                MessageBox.Show("לא כל השדות מולאו כראוי");
            else
                try
                {
                    bL.UpdateHost(host);
                    MessageBox.Show("העדכון בוצע בהצלחה");
                    new MainWindow().Show();
                    Close();
                }
                catch (ArgumentException err)
                {
                    MessageBox.Show(err.Message);
                }
            
        }
    }
}

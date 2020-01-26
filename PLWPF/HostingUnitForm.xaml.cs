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
using BE;
using BL;
using Utilities;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitForm.xaml
    /// </summary>
    public partial class HostingUnitForm : Window
    {
        IBL bL = BlFactory.getBl();
        HostingUnit hostingUnit = null;

        /// <summary>
        /// קונסטרקטור שמטפל ביצירת יחידת אירוח חדשה
        /// </summary>
        public HostingUnitForm()
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
            areaComboBox.ItemsSource = Enum.GetValues(typeof(Regions));
            typeComboBox.ItemsSource = Enum.GetValues(typeof(GRType));

            areaComboBox.SelectedIndex = 0;
            typeComboBox.SelectedIndex = 0;
            this.hostingUnit = new HostingUnit { OwnerKey = Cookies.LoginUserKey };
            TableGrid.DataContext = this.hostingUnit;
            SubmitButton.Click += AddHostingUnit;
        }

        /// <summary>
        /// קונסטרקטור שמטפל בעדכון יחידת אירוח
        /// </summary>
        /// <param name="hostingUnit">יחידת האירוח לעדכון</param>
        public HostingUnitForm(HostingUnit hostingUnit)
        {
            Cookies.PrevWindow = this.GetType().Name;
            InitializeComponent();
            areaComboBox.ItemsSource = Enum.GetValues(typeof(Regions));
            typeComboBox.ItemsSource = Enum.GetValues(typeof(GRType));

            this.hostingUnit = hostingUnit;
            areaComboBox.SelectedValue = hostingUnit.Area;
            typeComboBox.SelectedValue = hostingUnit.Type;
            TableGrid.DataContext = this.hostingUnit;
            SubmitButton.Content = "עדכן";
            SubmitButton.Click += UpdateHostingUnit;     
            
        }

        private void UpdateHostingUnit(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    bL.UpdateHostingUnit(this.hostingUnit);
                    MessageBox.Show("יחידת האירוח עודכנה בהצלחה!", "יחידה מספר " + this.hostingUnit.HostingUnitKey);
                    new PrivateZone().Show();
                    Close();
                }
                catch (ArgumentException err)
                {
                    MessageBox.Show(err.Message);
                }
                catch
                {
                    MessageBox.Show("אירעה שגיאה חמורה. אנא פנה למנהל האתר.");
                }
            }
        }

        private void AddHostingUnit(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    int key = bL.AddHostingUnit(hostingUnit);
                    MessageBox.Show("יחידת האירוח נוספה בהצלחה!", "יחידה מספר " + key);
                    new PrivateZone().Show();
                    Close();
                }
                catch (ArgumentException err)
                {
                    MessageBox.Show(err.Message);
                }
                catch
                {
                    MessageBox.Show("אירעה שגיאה חמורה. אנא פנה למנהל האתר.");
                }
            }
        }

        private bool ValidateForm()
        {
            if (!Tools.ValidateString(hostingUnitNameTextBox.Text))
                ErrorMessage.Text = "שם יחידת אירוח לא תקין";
            else if (!Tools.ValidateNumber(adultsTextBox.Text, 99) || !Tools.ValidateNumber(childrenTextBox.Text, 99)
                || int.Parse(childrenTextBox.Text) + int.Parse(adultsTextBox.Text) <= 0)
                ErrorMessage.Text = "מספר אורחים לא חוקי";
            else if (double.TryParse(priceTextBox.Text, out double x) && double.Parse(priceTextBox.Text) < 1)
                ErrorMessage.Text = "מחיר לא תקין";
            else
            {
                ErrorMessage.Text = "";
                return true;
            }
            return false;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateForm();
        }

    }
}

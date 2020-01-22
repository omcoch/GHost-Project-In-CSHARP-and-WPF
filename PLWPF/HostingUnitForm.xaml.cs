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

        public HostingUnitForm(HostingUnit hostingUnit = null)
        {
            InitializeComponent();
            areaComboBox.ItemsSource = Enum.GetValues(typeof(Regions));
            typeComboBox.ItemsSource = Enum.GetValues(typeof(GRType));

            if (hostingUnit != null) // טיפול בעדכון יחידת אירוח
            {
                TableGrid.DataContext = hostingUnit;             
                areaComboBox.SelectedValue = hostingUnit.Area;
                typeComboBox.SelectedValue = hostingUnit.Type;
                SubmitButton.Content = "עדכן";
                SubmitButton.Click += UpdateHostingUnit;
            }
            else // טיפול בהוספת יחידת אירוח חדשה
            {
                hostingUnit = new HostingUnit();
                TableGrid.DataContext = hostingUnit;                
                SubmitButton.Click += AddHostingUnit;
            }
        }

        private void UpdateHostingUnit(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    HostingUnit hu = (HostingUnit)TableGrid.DataContext;
                    bL.UpdateHostingUnit(hu);
                    MessageBox.Show("יחידת האירוח עודכנה בהצלחה!", "יחידה מספר " + hu.HostingUnitKey);
                    new PrivateZone(bL.GetHost(hu.OwnerKey)).Show();
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
                    int key=bL.AddHostingUnit((HostingUnit)TableGrid.DataContext);
                    MessageBox.Show("יחידת האירוח נוספה בהצלחה!", "יחידה מספר " + key);
                    new PrivateZone(bL.GetHost(((HostingUnit)TableGrid.DataContext).OwnerKey));
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
            else if (!Tools.ValidateNumber(priceTextBox.Text) || int.Parse(priceTextBox.Text) < 1)
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

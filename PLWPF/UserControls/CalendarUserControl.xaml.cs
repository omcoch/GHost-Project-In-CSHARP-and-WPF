using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for CalendarUserControl.xaml
    /// </summary>
    public partial class CalendarUserControl : UserControl
    {
        private Calendar MyCalendar;

        public CalendarUserControl()
        {
            InitializeComponent();
            MyCalendar = CreateCalendar();
            vbCalendar.Child = null; //מחיקה מהתצוגה של החלון הקודם
            vbCalendar.Child = MyCalendar;// הצגה של החלון הנוכחי שיצרנו
        }
        private Calendar CreateCalendar()
        {
            Calendar MonthlyCalendar = new Calendar
            {
                Name = "MonthlyCalendar",
                DisplayMode = CalendarMode.Month,
                SelectionMode = CalendarSelectionMode.SingleRange,
                IsTodayHighlighted = true
            };
            return MonthlyCalendar;
        }
        private void VbCalendar_GotMouseCapture_1(object sender, MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }
        public DateTime? GetEntryDate()
        {
            var l = MyCalendar.SelectedDates;
            if (l.Count > 0)
            {
                var myList = l.ToList();
                myList.Sort(); // צריך למיין את הרשימה למקרה שהימים בקלנדר נתפסו עם העכבר מהסוף להתחלה
                return myList.First();
            }
            return null;
        }
        public DateTime? GetReleaseDate()
        {
            var l = MyCalendar.SelectedDates;
            if (l.Count > 0)
            {
                var myList = l.ToList();
                myList.Sort();
                return myList.Last();
            }
            return null;
        }
    }
}

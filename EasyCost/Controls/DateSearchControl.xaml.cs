using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EasyCost.Controls
{
    public sealed partial class DateSearchControl : UserControl
    {
        public delegate void DateSelectedDelegate();
        public delegate void DisplayDateDelegate(DateTime fromDate, DateTime toDate);
        public event DateSelectedDelegate DateSelectedEvent;
        public event DisplayDateDelegate DisplayDateEvent;
        public DateSearchControl()
        {
            this.InitializeComponent();
        }

        private void fromDatePicker_Closed(object sender, object e)
        {
            DateSelectedEvent();
        }
        private void toDatePicker_Closed(object sender, object e)
        {
            DateSelectedEvent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DisplayDateEvent(fromDatePicker.Date.Value.DateTime, toDatePicker.Date.Value.DateTime);
        }
    }
}

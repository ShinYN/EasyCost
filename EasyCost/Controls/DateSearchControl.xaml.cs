using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

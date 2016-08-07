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
using Syncfusion.SfChart;
using System.Collections.ObjectModel;
using EasyCost.DataModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyCost.Pages.Statistics
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DailyStatisticsPage : Page
    {
        public ObservableCollection<CategoryChartModel> CategoryCharts { get; set; }

        public DailyStatisticsPage()
        {
            this.InitializeComponent();

            InitControls();

            pieSeries.ItemsSource = CategoryCharts;
        }

        private void InitControls()
        {
            CategoryCharts = new ObservableCollection<CategoryChartModel>()
            {
                new CategoryChartModel {Category = "교통비", Cost= 1000 },
                new CategoryChartModel {Category = "식비", Cost= 1000 },
                new CategoryChartModel {Category = "여행비", Cost= 1000 },
                new CategoryChartModel {Category = "의류비", Cost= 1000 }
            };
        }

        private void calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate != null)
            {
                costHistory.Display((DateTime)calendar.SelectedDate, true);
            }
        }
    }
}

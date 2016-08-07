using EasyCost.Helpers;
using EasyCost.Pages.Statistics;
using Syncfusion.UI.Xaml.Controls.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        const string COLOR_BACK_ENTER = "#FF0000";

        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        private void btnSearchDay_Click(object sender, RoutedEventArgs e)
        {
            if (statisticsFrame.CurrentSourcePageType != typeof(DailyStatisticsPage))
            {
                statisticsFrame.Navigate(typeof(DailyStatisticsPage));
                DisplayDailyFilterPanel();
            }
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (filterPanel.Visibility == Visibility.Collapsed)
            {
                filterPanel.Visibility = Visibility.Visible;
            }
            else
            {
                filterPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void btnSearchWeek_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayDailyFilterPanel()
        {
            filterPanel.Children.Clear();
            SfCalendar calendar = new SfCalendar();
            calendar.SelectionMode = Syncfusion.UI.Xaml.Controls.Input.SelectionMode.Single;
            calendar.ShowNavigationButton = true;
            calendar.Width = double.NaN;
            calendar.Height = double.NaN;

            filterPanel.Children.Add(calendar);
        }
    }
}

using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        public ObservableCollection<CostStatisticsModel> mStatisticsModel = new ObservableCollection<CostStatisticsModel>();

        public StatisticsPage()
        {
            this.InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            
        }

        private void DisplayStatisticsDataByWeek()
        {
            mStatisticsModel.Clear();
            DateTime currentDateTime = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            for (int i = 0; i < 7; i++)
            {
                mStatisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = currentDateTime,
                    ToDate = currentDateTime,
                    CostInfo = CostManager.GetCostInfo(currentDateTime),
                    DisplayString = currentDateTime.ToString("MM/dd")
                });

                currentDateTime = currentDateTime.AddDays(1);
            }
            
            //costChart.Header = currentDateTime.AddDays(-7).ToString("yyyy-MM-dd") + " ~ " + currentDateTime.ToString("yyyy-MM-dd");
            chartColumn.ItemsSource = mStatisticsModel;
        }
        private void DisplayStatisticsDataByMonth()
        {
            mStatisticsModel.Clear();
            DateTime currentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            for (int i = 0; i < DateTime.Now.Day; i++)
            {
                mStatisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = currentDateTime,
                    ToDate = currentDateTime,
                    CostInfo = CostManager.GetCostInfo(currentDateTime),
                    DisplayString = currentDateTime.ToString("dd")
                });

                currentDateTime = currentDateTime.AddDays(1);
            }

            chartColumn.ItemsSource = mStatisticsModel;
        }

        private void btnSearchWeek_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatisticsDataByWeek();
        }
        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatisticsDataByMonth();
        }
        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            List<CostInfo> costInfo = CostManager.GetCostInfo(InquiryType.Year, true);
        }
        private void btnSearchCustom_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

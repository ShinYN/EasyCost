﻿using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml.Media;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        private InquiryType _currentInquiryType;
        Button _currentButton = null;
        List<Button> _searchButtonList = new List<Button>();
        private ObservableCollection<CostStatisticsModel> _statisticsModel = new ObservableCollection<CostStatisticsModel>();
        
        public StatisticsPage()
        {
            this.InitializeComponent();
            InitializeControls();
        }
        
        private void InitializeControls()
        {
            _searchButtonList.Clear();
            _searchButtonList.Add(btnSearchWeek);
            _searchButtonList.Add(btnSearchMonth);
            _searchButtonList.Add(btnSearchYear);

            topCostChart.Title = "항목별 지출 금액";

            topSubCostChart.ItemSelectedEvent += delegate (CategoryCostModel costModel) { };
            topCostChart.ItemSelectedEvent += delegate (CategoryCostModel costModel)
                {
                    DisplayCostSubCategory(costModel.Category);
                };

            _currentButton = btnSearchWeek;
            SetSearchButtonColor(_currentButton);

            DisplayStatisticsDataByWeek();
        }

        private void DisplayStatisticsDataByWeek()
        {
            DisplayStatisticsDataByWeek(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        }
        private void DisplayStatisticsDataByWeek(DateTime aStartDayOfWeek)
        {
            _statisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            for (int i = 0; i < 7; i++)
            {
                _statisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = aStartDayOfWeek,
                    ToDate = aStartDayOfWeek,
                    CostInfo = CostManager.GetCostInfo(aStartDayOfWeek),
                    DisplayString = aStartDayOfWeek.ToString("MM/dd")
                });

                foreach (CostInfo costInfo in _statisticsModel[i].CostInfo)
                {
                    if (categoryCostDic.ContainsKey(costInfo.Category))
                    {
                        categoryCostDic[costInfo.Category] += costInfo.Cost;
                    }
                    else
                    {
                        categoryCostDic.Add(costInfo.Category, costInfo.Cost);
                    }
                }

                aStartDayOfWeek = aStartDayOfWeek.AddDays(1);
            }

            txtInquiryTarget.Text = aStartDayOfWeek.AddDays(-7).ToString("yyyy/MM/dd")
                                  + " - "
                                  + aStartDayOfWeek.AddDays(-1).ToString("yyyy/MM/dd");
            cashColumn.ItemsSource = _statisticsModel;
            cardColumn.ItemsSource = _statisticsModel;
            costHistoryXAxis.Header = "기간(일)";

            DisplayCostCategory(categoryCostDic);
            DisplayTotalCostInfo();

            _currentInquiryType = InquiryType.Week;
        }
        private void DisplayStatisticsDataByMonth()
        {
            DisplayStatisticsDataByMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        }
        private void DisplayStatisticsDataByMonth(DateTime aStartDayOfMonth)
        {
            _statisticsModel.Clear();

            var endDayOfMonth = aStartDayOfMonth.AddMonths(1).AddDays(-1).Day;
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            for (int i = 0; i < endDayOfMonth; i++)
            {
                _statisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = aStartDayOfMonth,
                    ToDate = aStartDayOfMonth,
                    CostInfo = CostManager.GetCostInfo(aStartDayOfMonth),
                    DisplayString = aStartDayOfMonth.ToString("dd")
                });

                foreach (CostInfo costInfo in _statisticsModel[i].CostInfo)
                {
                    if (categoryCostDic.ContainsKey(costInfo.Category))
                    {
                        categoryCostDic[costInfo.Category] += costInfo.Cost;
                    }
                    else
                    {
                        categoryCostDic.Add(costInfo.Category, costInfo.Cost);
                    }
                }

                aStartDayOfMonth = aStartDayOfMonth.AddDays(1);
            }

            txtInquiryTarget.Text = aStartDayOfMonth.AddDays(-1 * endDayOfMonth).ToString("yyyy/MM/dd")
                                  + " - "
                                  + aStartDayOfMonth.AddDays(-1).ToString("yyyy/MM/dd");

            cashColumn.ItemsSource = _statisticsModel;
            cardColumn.ItemsSource = _statisticsModel;
            costHistoryXAxis.Header = "기간(일)";

            DisplayCostCategory(categoryCostDic);
            DisplayTotalCostInfo();

            _currentInquiryType = InquiryType.Month;
        }
        private void DisplayStatisticsDataByYear(DateTime aStartDayOfYear)
        {
            _statisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            DateTime aEndDayOfMonth = new DateTime();
            for (int i = 0; i < 12; i++)
            {
                aEndDayOfMonth = new DateTime(aStartDayOfYear.Year, aStartDayOfYear.Month, DateTime.DaysInMonth(aStartDayOfYear.Year, aStartDayOfYear.Month));
                _statisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Month,
                    FromDate = aStartDayOfYear,
                    ToDate = aEndDayOfMonth,
                    CostInfo = CostManager.GetCostInfo(aStartDayOfYear, aEndDayOfMonth),
                    DisplayString = aStartDayOfYear.Month.ToString()
                });

                foreach (CostInfo costInfo in _statisticsModel[i].CostInfo)
                {
                    if (categoryCostDic.ContainsKey(costInfo.Category))
                    {
                        categoryCostDic[costInfo.Category] += costInfo.Cost;
                    }
                    else
                    {
                        categoryCostDic.Add(costInfo.Category, costInfo.Cost);
                    }
                }

                aStartDayOfYear = aStartDayOfYear.AddMonths(1);
            }
            txtInquiryTarget.Text = new DateTime(aStartDayOfYear.Year - 1, 1, 1).ToString("yyyy/MM/dd")
                                  + " - "
                                  + aEndDayOfMonth.ToString("yyyy/MM/dd");

            cashColumn.ItemsSource = _statisticsModel;
            cardColumn.ItemsSource = _statisticsModel;
            costHistoryXAxis.Header = "기간(월)";

            DisplayCostCategory(categoryCostDic);
            DisplayTotalCostInfo();

            _currentInquiryType = InquiryType.Year;
        }
        private void DisplayStatisticsDataBySpecificDate()
        {
            _statisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            DateTime currentDateTime = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime toDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 20);
            for (int i = 0; i < (toDateTime - currentDateTime).TotalDays ; i++)
            {
                _statisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = currentDateTime,
                    ToDate = currentDateTime,
                    CostInfo = CostManager.GetCostInfo(currentDateTime),
                    DisplayString = currentDateTime.ToString("dd")
                });

                foreach (CostInfo costInfo in _statisticsModel[i].CostInfo)
                {
                    if (categoryCostDic.ContainsKey(costInfo.Category))
                    {
                        categoryCostDic[costInfo.Category] += costInfo.Cost;
                    }
                    else
                    {
                        categoryCostDic.Add(costInfo.Category, costInfo.Cost);
                    }
                }

                currentDateTime = currentDateTime.AddDays(1);
            }

            cashColumn.ItemsSource = _statisticsModel;
            cardColumn.ItemsSource = _statisticsModel;
            costHistoryXAxis.Header = "기간(일)";
        }

        private void DisplayCostCategory(Dictionary<string, int> aCategoryCostDic)
        {
            int costSum = aCategoryCostDic.Values.Sum();
            int index = 1;
            List<CategoryCostModel> categoryCostModels = new List<CategoryCostModel>();
            foreach (KeyValuePair<string, int> categoryCostItem in aCategoryCostDic.OrderByDescending(x => x.Value))
            {
                categoryCostModels.Add(new CategoryCostModel { Index = index,
                                                               Category = categoryCostItem.Key,
                                                               CostRatio = (categoryCostItem.Value * 100) / costSum,
                                                               Cost = categoryCostItem.Value });
                index++;
            }

            topCostChart.Display(categoryCostModels);

            if (categoryCostModels.Count == 0)
            {
                topSubCostChart.Clear();
            }
            else
            {
                DisplayCostSubCategory(categoryCostModels.Select(x => x.Category).First());
            }
        }
        private void DisplayCostSubCategory(string aCategory)
        {
            Dictionary<string, int> subCategoryCostDic = new Dictionary<string, int>();
            foreach (CostStatisticsModel item in _statisticsModel)
            {
                foreach (CostInfo costInfo in item.CostInfo.Where(elem => elem.Category == aCategory))
                {
                    if (subCategoryCostDic.ContainsKey(costInfo.SubCategory))
                    {
                        subCategoryCostDic[costInfo.SubCategory] += costInfo.Cost;
                    }
                    else
                    {
                        subCategoryCostDic.Add(costInfo.SubCategory, costInfo.Cost);
                    }
                }
            }

            int costSum = subCategoryCostDic.Values.Sum();
            int index = 1;
            List<CategoryCostModel> categoryCostModes = new List<CategoryCostModel>();
            foreach (KeyValuePair<string, int> subCategoryCostItem in subCategoryCostDic.OrderByDescending(x => x.Value))
            {
                categoryCostModes.Add(new CategoryCostModel() { Index = index,
                                                                Category = subCategoryCostItem.Key,
                                                                CostRatio = (subCategoryCostItem.Value * 100) / costSum,
                                                                Cost = subCategoryCostItem.Value });
            }

            topSubCostChart.Display(categoryCostModes);
            topSubCostChart.Title = $"세부 지출 리스트- {aCategory}";
        }

        private void DisplayTotalCostInfo()
        {
            txtTotalCost.Text = _statisticsModel.Sum(x => x.Cost).ToString("#,##0");
            txtCardCost.Text = _statisticsModel.Sum(x => x.CardCost).ToString("#,##0");
            txtCashCost.Text = _statisticsModel.Sum(x => x.CashCost).ToString("#,##0");
        }

        private void DisplayDetailStatisticsData(CostStatisticsModel aCostStatisticsModel)
        {
            if (aCostStatisticsModel.InquiryType == InquiryType.Today)
            {
                txtSubInquriyDate.Text = aCostStatisticsModel.FromDate.ToString("yyyy-MM-dd");
            }
            else  // Case of month
            {
                txtSubInquriyDate.Text = aCostStatisticsModel.FromDate.ToString("yyyy-MM-dd")
                                       + " ~ "
                                       + aCostStatisticsModel.ToDate.ToString("yyyy-MM-dd");
            }
            txtSubTotalCost.Text = aCostStatisticsModel.Cost.ToString("#,##0");
            txtSubCardCost.Text = aCostStatisticsModel.CardCost.ToString("#,##0");
            txtSubCashCost.Text = aCostStatisticsModel.CashCost.ToString("#,##0");

            subCostCategoryChart.ItemsSource = aCostStatisticsModel.CostInfo.GroupBy(x => new { x.Category })
                                                                            .Select(x => new { Category = x.Key.Category, Cost = x.Sum(y => y.Cost) })
                                                                            .OrderBy(x => x.Cost)
                                                                            .ToList();
        }

        private void btnSearchWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            DisplayStatisticsDataByWeek(DateTime.Now.StartOfWeek(DayOfWeek.Monday));
        }
        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            DisplayStatisticsDataByMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
        }
        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            DisplayStatisticsDataByYear(new DateTime(DateTime.Now.Year, 1, 1));
        }
        private void btnSearchCustom_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatisticsDataBySpecificDate();
        }

        private void costHistoryChart_SelectionChanged(object sender, Syncfusion.UI.Xaml.Charts.ChartSelectionChangedEventArgs e)
        {
            inputCostMainSplitView.IsPaneOpen = !inputCostMainSplitView.IsPaneOpen;
            if (inputCostMainSplitView.IsPaneOpen && e.SelectedSegment != null)
            {
                DisplayDetailStatisticsData((CostStatisticsModel)e.SelectedSegment.Item);
            }
        }
        private void topCostChart_SelectionChanged(object sender, Syncfusion.UI.Xaml.Charts.ChartSelectionChangedEventArgs e)
        {
            if (e.SelectedSegment == null)
            {
                return;
            }
            
            DisplayCostSubCategory(((CategoryCostModel)e.SelectedSegment.Item).Category);
            //topSubCategoryCostBar.Interior = e.SelectedSegment.Interior;
        }

        private void btnMoveNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentInquiryType == InquiryType.Week)
            {
                DisplayStatisticsDataByWeek(_statisticsModel[0].FromDate.AddDays(7).StartOfWeek(DayOfWeek.Monday));
            }
            else if (_currentInquiryType == InquiryType.Month)
            {
                DisplayStatisticsDataByMonth(_statisticsModel[0].FromDate.AddMonths(1));
            }
            else if (_currentInquiryType == InquiryType.Year)
            {
                DisplayStatisticsDataByYear(_statisticsModel[0].FromDate.AddYears(1));
            }
        }
        private void btnMovePrev_Click(object sender, RoutedEventArgs e)
        {
            if (_currentInquiryType == InquiryType.Week)
            {
                DisplayStatisticsDataByWeek(_statisticsModel[0].FromDate.AddDays(-7).StartOfWeek(DayOfWeek.Monday));
            }
            else if (_currentInquiryType == InquiryType.Month)
            {
                DisplayStatisticsDataByMonth(_statisticsModel[0].FromDate.AddMonths(-1));
            }
            else if (_currentInquiryType == InquiryType.Year)
            {
                DisplayStatisticsDataByYear(_statisticsModel[0].FromDate.AddYears(-1));
            }
        }

        private void btnSearchButton_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SetSearchButtonColor((Button)sender);
        }

        private void btnSearchButton_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            SetSearchButtonColor(_currentButton);
        }

        private void SetSearchButtonColor(Button aMouseOverButton)
        {
            const string main_color = "767676";

            foreach (var button in _searchButtonList)
            {
                if (button == _currentButton || button == aMouseOverButton)
                {
                    button.Background = MyColorHelper.GetSolidColorBrush(main_color);
                    button.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                }
                else
                {
                    button.Background = new SolidColorBrush(Windows.UI.Colors.White);
                    button.Foreground = MyColorHelper.GetSolidColorBrush(main_color);
                }
            }
        }
    }
}

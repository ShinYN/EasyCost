using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        private ObservableCollection<CostStatisticsModel> mStatisticsModel = new ObservableCollection<CostStatisticsModel>();
        
        public StatisticsPage()
        {
            this.InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            DisplayStatisticsDataByWeek();
        }

        private void DisplayStatisticsDataByWeek()
        {
            mStatisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
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

                foreach (CostInfo costInfo in mStatisticsModel[i].CostInfo)
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

            txtInquiryTarget.Text = currentDateTime.AddDays(-7).ToString("yyyy/MM/dd")
                                  + " - " 
                                  + currentDateTime.AddDays(-1).ToString("yyyy/MM/dd");
            cashColumn.ItemsSource = mStatisticsModel;
            cardColumn.ItemsSource = mStatisticsModel;
            costHistoryXAxis.Header = "기간(일)";

            DisplayTop5CostCategory(categoryCostDic);
            DisplayTotalCostInfo();
        }
        private void DisplayStatisticsDataByMonth()
        {
            mStatisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
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

                foreach (CostInfo costInfo in mStatisticsModel[i].CostInfo)
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

            txtInquiryTarget.Text = currentDateTime.AddDays(-1 * DateTime.Now.Day).ToString("yyyy/MM/dd")
                                  + " - "
                                  + currentDateTime.AddDays(-1).ToString("yyyy/MM/dd");

            cashColumn.ItemsSource = mStatisticsModel;
            cardColumn.ItemsSource = mStatisticsModel;
            costHistoryXAxis.Header = "기간(일)";

            DisplayTop5CostCategory(categoryCostDic);
            DisplayTotalCostInfo();
        }
        private void DisplayStatisticsDataByYear()
        {
            mStatisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            DateTime fromDateTime = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime toDateTime = new DateTime();
            for (int i = 0; i < DateTime.Now.Month; i++)
            {
                toDateTime = new DateTime(fromDateTime.Year, fromDateTime.Month, DateTime.DaysInMonth(fromDateTime.Year, fromDateTime.Month));
                mStatisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Month,
                    FromDate = fromDateTime,
                    ToDate = toDateTime,
                    CostInfo = CostManager.GetCostInfo(fromDateTime, toDateTime),
                    DisplayString = fromDateTime.Month.ToString()
                });

                foreach (CostInfo costInfo in mStatisticsModel[i].CostInfo)
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

                fromDateTime = fromDateTime.AddMonths(1);
            }
            txtInquiryTarget.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy/MM/dd")
                                  + " - "
                                  + toDateTime.ToString("yyyy/MM/dd");

            cashColumn.ItemsSource = mStatisticsModel;
            cardColumn.ItemsSource = mStatisticsModel;
            costHistoryXAxis.Header = "기간(월)";

            DisplayTop5CostCategory(categoryCostDic);
            DisplayTotalCostInfo();
        }
        private void DisplayStatisticsDataBySpecificDate()
        {
            mStatisticsModel.Clear();
            Dictionary<string, int> categoryCostDic = new Dictionary<string, int>();
            DateTime currentDateTime = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime toDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 20);
            for (int i = 0; i < (toDateTime - currentDateTime).TotalDays ; i++)
            {
                mStatisticsModel.Add(new CostStatisticsModel()
                {
                    InquiryType = InquiryType.Today,
                    FromDate = currentDateTime,
                    ToDate = currentDateTime,
                    CostInfo = CostManager.GetCostInfo(currentDateTime),
                    DisplayString = currentDateTime.ToString("dd")
                });

                foreach (CostInfo costInfo in mStatisticsModel[i].CostInfo)
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

            cashColumn.ItemsSource = mStatisticsModel;
            cardColumn.ItemsSource = mStatisticsModel;
            costHistoryXAxis.Header = "기간(일)";
        }

        private void DisplayTop5CostCategory(Dictionary<string, int> aCategoryCostDic)
        {
            List<CategoryCostModel> categoryCostModels = new List<CategoryCostModel>();
            foreach (KeyValuePair<string, int> categoryCostItem in aCategoryCostDic.OrderBy(x => x.Value))
            {
                categoryCostModels.Add(new CategoryCostModel { Category = categoryCostItem.Key, Cost = categoryCostItem.Value });

                if (categoryCostModels.Count == 5)
                {
                    break;
                }
            }

            topCategoryCostBar.ItemsSource = categoryCostModels;

            if (categoryCostModels.Count != 0)
            {
                DisplayTop5CostSubCategory(categoryCostModels.Select(x => x.Category).Last());
            }
        }
        private void DisplayTop5CostSubCategory(string aCategory)
        {
            Dictionary<string, int> subCategoryCostDic = new Dictionary<string, int>();
            foreach (CostStatisticsModel item in mStatisticsModel)
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

            List<SubCategoryCostModel> subCategoryCostModes = new List<SubCategoryCostModel>();
            foreach (KeyValuePair<string, int> subCategoryCostItem in subCategoryCostDic.OrderBy(x => x.Value))
            {
                subCategoryCostModes.Add(new SubCategoryCostModel() { Category = aCategory, SubCategory = subCategoryCostItem.Key, Cost = subCategoryCostItem.Value });

                if (subCategoryCostModes.Count == 5)
                {
                    break;
                }
            }

            topSubCategoryCostBar.ItemsSource = subCategoryCostModes;
            topSubCategoryCostBar.Label = $"Top 5 세부 지출 - {aCategory}";
        }

        private void DisplayTotalCostInfo()
        {
            txtTotalCost.Text = mStatisticsModel.Sum(x => x.Cost).ToString("#,##0");
            txtCardCost.Text = mStatisticsModel.Sum(x => x.CardCost).ToString("#,##0");
            txtCashCost.Text = mStatisticsModel.Sum(x => x.CashCost).ToString("#,##0");
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
            DisplayStatisticsDataByWeek();
        }
        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatisticsDataByMonth();
        }
        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatisticsDataByYear();
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
            DisplayTop5CostSubCategory(((CategoryCostModel)e.SelectedSegment.Item).Category);
        }
    }
}

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
using EasyCost.Databases.TableModels;

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
        }

        public void Display(DateTime aDate)
        {
            costHistory.Display(aDate, true);
            DisplayCategoryChart();
        }

        private void DisplayCategoryChart()
        {
            if (costHistory.CostInfo == null)
            {
                return;
            }
            categoryChart.ItemsSource = costHistory.CostInfo.GroupBy(elem => new { elem.Category })
                                                            .Select(x => new CategoryChartModel { Category = x.Key.Category, Cost = x.Sum(y => y.Cost) })
                                                            .ToList();

            if (costHistory.CostInfo.Count() == 0)
            {
                DisplaySubCategoryChart(string.Empty);
            }
            else
            {
                DisplaySubCategoryChart(costHistory.CostInfo[0].Category);
            }
        }
        private void DisplaySubCategoryChart(string aSubCategory)
        {
            txtSubCategoryTitle.Text = "세부 분류 별 내역: " + aSubCategory;
            subCategoryChart.ItemsSource = costHistory.CostInfo.Where(elem => elem.Category == aSubCategory)
                                                               .GroupBy(elem => new { elem.SubCategory })
                                                               .Select(x => new SubCategoryChartModel { SubCategory = x.Key.SubCategory, Cost = x.Sum(y => y.Cost) })
                                                               .ToList();
        }

        private void SfChart_SelectionChanged(object sender, Syncfusion.UI.Xaml.Charts.ChartSelectionChangedEventArgs e)
        {
            if (e.SelectedSegment == null)
            {
                return;
            }

            dynamic category = e.SelectedSegment.Item;
            DisplaySubCategoryChart(category.Category);
        }
    }
}

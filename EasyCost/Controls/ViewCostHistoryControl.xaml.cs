using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.Types;
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
    public sealed partial class ViewCostHistoryControl : UserControl
    {
        public ViewCostHistoryControl()
        {
            this.InitializeComponent();
        }

        public List<CostInfo> CostInfo { get; private set; }

        /// <summary>
        /// 지출 내역을 표시하는 함수
        /// </summary>
        /// <param name="aDisplayData">YYYYMMDD 형식</param>
        public void Display(DateTime aDisplayDate, bool aSelectGroupBy = false)
        {
            lsvHistory.Items.Clear();

            CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.ToString("yyyyMMdd") == aDisplayDate.ToString("yyyyMMdd")).ToList();
            DisplayCostInfoToListView();
        }
        public void Display(InquiryType aInquiryType, bool aSelectGroupBy = false)
        {
            lsvHistory.Items.Clear();

            if (aInquiryType == InquiryType.Today)
            {
                CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).ToList();
            }
            else if (aInquiryType == InquiryType.Week)
            {
                CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate > DateTime.Now.AddDays(-7)).ToList();
            }
            else if (aInquiryType == InquiryType.Month)
            {
                CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate > DateTime.Now.AddMonths(-1)).ToList();
            }
            else if (aInquiryType == InquiryType.Year)
            {
                CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate > DateTime.Now.AddYears(-1)).ToList();
            }
            else   // Case of all
            {
                CostInfo = DBConnHandler.Cost.GetCostInfo(aSelectGroupBy);
            }

            DisplayCostInfoToListView();
        }

        private void DisplayCostInfoToListView()
        {
            if (CostInfo == null)
            {
                return;
            }

            int totalCost = CostInfo.Sum(elem => elem.Cost);
            int index = 1;
            CostInfo.OrderByDescending(elem => elem.Cost).ToList().ForEach(elem =>
                {
                    lsvHistory.Items.Add(new
                    {
                        Id = index,
                        CostDate = elem.CostDate.ToString("yyyy-MM-dd"),
                        Category = elem.Category,
                        SubCategory = elem.SubCategory,
                        CostType = elem.CostType,
                        Cost = elem.Cost.ToString("#,##0").PadLeft(10, ' ') + "원" + " (" + ((double)elem.Cost * 100 / (double)totalCost).ToString("#0.#") + "%)",
                        Description = elem.Description
                    });

                    index++;
                }
            );

            lblTotalCost.Text = totalCost.ToString("#,##0");
        }
    }
}

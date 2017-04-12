using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        public delegate void CostItemSelected(CostHistoryModel costInfo);
        public event CostItemSelected CostItemSelectedEvent;

        public ViewCostHistoryControl()
        {
            this.InitializeComponent();
        }

        public List<CostInfo> CostInfo { get; private set; }
        public CostHistoryModel SelectedItem
        {
            get
            {
                if (lsvHistory.SelectedItem == null)
                {
                    return null;
                }
                else
                {
                    return (CostHistoryModel)lsvHistory.SelectedItem;
                }
            }
        }
        public void Display(DateTime aDisplayDate, bool aSelectGroupBy = false)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aDisplayDate, aSelectGroupBy);
            DisplayCostInfoToListView(aSelectGroupBy);
        }
        public void Display(InquiryType aInquiryType, bool aSelectGroupBy = false)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aInquiryType, aSelectGroupBy);
            DisplayCostInfoToListView(aSelectGroupBy);
        }
        public void Display(DateTime aFromDate, DateTime aToDate, bool aSelectGroupBy = false)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aFromDate, aToDate, aSelectGroupBy);
            DisplayCostInfoToListView(aSelectGroupBy);
        }

        private void DisplayCostInfoToListView(bool aSelectGroupBy)
        {
            if (CostInfo == null)
            {
                return;
            }

            int totalCost = CostInfo.Sum(elem => elem.Cost);
            int index = 1;

            var orderedCostInfo = (aSelectGroupBy) ? CostInfo.OrderByDescending(elem => elem.Cost) : CostInfo.OrderByDescending(elem => elem.CostDate);

            orderedCostInfo.ToList().ForEach(elem =>
                {
                    lsvHistory.Items.Add(new CostHistoryModel
                    {
                        Index = index,
                        Id = elem.Id,
                        CostDate = elem.CostDate.ToString("yyyy-MM-dd"),
                        CostDateTime = elem.CostDate,
                        Category = elem.Category,
                        SubCategory = elem.SubCategory,
                        CostType = elem.CostType,
                        Cost = elem.Cost,
                        CostString = elem.Cost.ToString("#,##0").PadLeft(10, ' ') + "원",
                        Percentage = ((double)elem.Cost * 100 / (double)totalCost).ToString("#0.#") + "%",
                        Description = elem.Description
                    });

                    index++;
                }
            );

            lblTotalCost.Text = totalCost.ToString("#,##0");
        }

        private void lsvHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null && e.ClickedItem.GetType() == typeof(CostHistoryModel))
            {
                CostItemSelectedEvent((CostHistoryModel)e.ClickedItem);
            }
        }
    }
}

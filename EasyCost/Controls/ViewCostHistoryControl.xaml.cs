using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EasyCost.Controls
{
    public sealed partial class ViewCostHistoryControl : UserControl
    {
        public delegate void CostItemSelected(CostHistoryModel costInfo);
        public event CostItemSelected CostItemSelectedEvent;

        private List<CardMaster> _cardList;

        public ViewCostHistoryControl()
        {
            this.InitializeComponent();

            _cardList = CardManager.GetCardList();
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
        public void Display(DateTime aDisplayDate)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aDisplayDate, string.Empty);

            InitInquiryComboBoxs();
            DisplayCostInfoToListView();
        }
        public void Display(InquiryType aInquiryType)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aInquiryType, string.Empty);

            InitInquiryComboBoxs();
            DisplayCostInfoToListView();
        }
        public void Display(DateTime aFromDate, DateTime aToDate)
        {
            lsvHistory.Items.Clear();

            CostInfo = CostManager.GetCostInfo(aFromDate, aToDate, string.Empty);

            InitInquiryComboBoxs();
            DisplayCostInfoToListView();
        }

        private void DisplayCostInfoToListView()
        {
            if (CostInfo == null)
            {
                return;
            }

            var index = 1;
            var tempCostInfo = CostInfo.AsEnumerable<CostInfo>();
            if ((string)cboCategoryType.SelectedValue != string.Empty)
            {
                if ((string)cboCategoryType.SelectedValue == "지출")
                {
                    tempCostInfo = tempCostInfo.Where(elem => elem.CategoryType == CategoryType.Expense);
                }
                else
                {
                    tempCostInfo = tempCostInfo.Where(elem => elem.CategoryType == CategoryType.Income);
                }   
            }
            if ((string)cboCategory.SelectedValue != string.Empty)
            {
                tempCostInfo = tempCostInfo.Where(elem => elem.Category == (string)cboCategory.SelectedValue);
            }
            if ((string)cboSubCategory.SelectedValue != string.Empty)
            {
                tempCostInfo = tempCostInfo.Where(elem => elem.SubCategory == (string)cboSubCategory.SelectedValue);
            }
            if ((string)cboCostType.SelectedValue != string.Empty)
            {
                tempCostInfo = tempCostInfo.Where(elem => elem.CostType == (string)cboCostType.SelectedValue);
            }
            if ((string)cboCostCard.SelectedValue != string.Empty)
            {
                tempCostInfo = tempCostInfo.Where(elem => elem.CostCard == (string)cboCostCard.SelectedValue);
            }
            if ((string)cboCardCompany.SelectedValue != string.Empty)
            {
                tempCostInfo = tempCostInfo.Where(elem => GetCardCompany(elem.CostCard) == (string)cboCardCompany.SelectedValue);
            }
            if (string.IsNullOrWhiteSpace(txtSearchDescrtipion.Text) == false)
            {
                tempCostInfo = tempCostInfo.Where(elem => elem.Description.Contains(txtSearchDescrtipion.Text.Trim()));
            }

            var orderedCostInfo = tempCostInfo.OrderByDescending(elem => elem.CostDate);
            var totalIncomeCost = tempCostInfo.Where(elem => elem.CategoryType == CategoryType.Income).Sum(elem => elem.Cost);
            var totalExpenseCost = tempCostInfo.Where(elem => elem.CategoryType == CategoryType.Expense).Sum(elem => elem.Cost);
            var totalCost = totalIncomeCost - totalExpenseCost;

            lsvHistory.Items.Clear();
            orderedCostInfo.ToList().ForEach(elem =>
                {
                    lsvHistory.Items.Add(new CostHistoryModel
                    {
                        Index = index,
                        Id = elem.Id,
                        CostDate = elem.CostDate.ToString("yyyy-MM-dd"),
                        CostDateTime = elem.CostDate,
                        CategoryType = elem.CategoryType,
                        CategoryTypeString = (elem.CategoryType == CategoryType.Expense) ? "지출" : "수입",
                        Category = elem.Category,
                        SubCategory = elem.SubCategory,
                        CostType = elem.CostType,
                        CostCard = elem.CostCard,
                        CardCompany = (elem.CostCard == string.Empty) 
                            ? string.Empty 
                            : _cardList.Where(x => x.CardName == elem.CostCard).Select(x => x.Company).FirstOrDefault(),
                        Cost = elem.Cost,
                        CostString = elem.Cost.ToString("#,##0").PadLeft(10, ' ') + "원",
                        Percentage = (elem.CategoryType == CategoryType.Expense) ? ((double)elem.Cost * 100 / (double)totalExpenseCost).ToString("#0.#") + "%" : string.Empty,
                        Description = elem.Description
                    });

                    index++;
                }
            );
            
            lblTotalIncomeCost.Text = totalIncomeCost.ToString("#,##0");
            lblTotalExpenseCost.Text = totalExpenseCost.ToString("#,##0");
            lblTotalCost.Text = totalCost.ToString("#,##0");
        }

        private void InitInquiryComboBoxs()
        {
            InitCategoryTypeComboBox();
            InitCategoryComboBox();
            InitSubCategoryComboBox();
            InitCostTypeComboBox();
            InitCostCardComboBox();
            InitCardCompanyComboBox();
        }
        private void InitCategoryTypeComboBox()
        {
            cboCategoryType.Items.Clear();
            cboCategoryType.Items.Add(string.Empty);
            foreach (var item in CostInfo.GroupBy(x => x.CategoryType).Select(x => x.First()))
            {
                if (item.CategoryType == CategoryType.Expense)
                {
                    cboCategoryType.Items.Add("지출");
                }
                else
                {
                    cboCategoryType.Items.Add("수입");
                }
            }

            cboCategoryType.SelectedIndex = 0;
        }
        private void InitCategoryComboBox()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add(string.Empty);
            CostInfo.GroupBy(x => x.Category).Select(x => x.First()).ToList()
                                             .ForEach(x => cboCategory.Items.Add(x.Category));
            cboCategory.SelectedIndex = 0;
        }
        private void InitSubCategoryComboBox()
        {
            cboSubCategory.Items.Clear();
            cboSubCategory.Items.Add(string.Empty);
            if (cboCategory.SelectedValue as string != string.Empty)
            {
                CostInfo.Where(x => x.Category == cboCategory.SelectedValue as string)
                        .GroupBy(x => x.SubCategory).Select(x => x.First()).ToList()
                        .ForEach(x => cboSubCategory.Items.Add(x.SubCategory));
            }

            cboSubCategory.SelectedIndex = 0;
        }
        private void InitCostTypeComboBox()
        {
            cboCostType.Items.Clear();
            cboCostType.Items.Add(string.Empty);
            CostInfo.GroupBy(x => x.CostType).Select(x => x.First()).ToList()
                                             .ForEach(x => cboCostType.Items.Add(x.CostType));
            cboCostType.SelectedIndex = 0;
        }
        private void InitCostCardComboBox()
        {
            cboCostCard.Items.Clear();
            cboCostCard.Items.Add(string.Empty);
            if (cboCostType.SelectedValue as string != "현금")
            {
                CostInfo.Where(x => x.CostCard != string.Empty)
                        .GroupBy(x => x.CostCard)
                        .Select(x => x.First()).ToList()
                        .ForEach(x => cboCostCard.Items.Add(x.CostCard));
            }

            cboCostCard.SelectedIndex = 0;
        }
        private void InitCardCompanyComboBox()
        {
            cboCardCompany.Items.Clear();
            cboCardCompany.Items.Add(string.Empty);
            if (cboCostType.SelectedValue as string != "현금")
            {
                _cardList.ForEach(x => cboCardCompany.Items.Add(x.Company));
            }

            cboCardCompany.SelectedIndex = 0;
        }

        private string GetCardCompany(string cardName)
        {
            return _cardList.Where(x => x.CardName == cardName).Select(x => x.Company).FirstOrDefault();
        }

        private void lsvHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null && e.ClickedItem.GetType() == typeof(CostHistoryModel))
            {
                CostItemSelectedEvent((CostHistoryModel)e.ClickedItem);
            }
        }

        private void cboCategoryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayCostInfoToListView();
        }
        private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitSubCategoryComboBox();
            DisplayCostInfoToListView();
        }
        private void cboSubCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayCostInfoToListView();
        }
        private void cboCostType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitCostCardComboBox();
            InitCardCompanyComboBox();
            DisplayCostInfoToListView();
        }
        private void txtSearchDescrtipion_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayCostInfoToListView();
        }
        private void cboCostCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayCostInfoToListView();
        }
        private void cboCardCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayCostInfoToListView();
        }
    }
}

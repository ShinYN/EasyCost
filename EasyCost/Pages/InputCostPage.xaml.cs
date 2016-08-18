using EasyCost.Bases.Login;
using EasyCost.Controls;
using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
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

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class InputCostPage : Page
    {
        InquiryType lastInquiryType;
        public InputCostPage()
        {
            this.InitializeComponent();

            costHistory.CostItemSelectedEvent += new Controls.ViewCostHistoryControl.CostItemSelected(DisplayInputCostForUpdate);
            dateSearchControl.DateSelectedEvent += () => Flyout.ShowAttachedFlyout(btnSearchCustom);
            dateSearchControl.DisplayDateEvent += (x, y) => costHistory.Display(x, y, false);
            Flyout.SetAttachedFlyout(btnSearchCustom, searchCustomFlyout);

            InitControls();
            DisplayCostHistory();
        }

        private void InitControls()
        {
            lastInquiryType = InquiryType.Today;
            InitInputCostControls();
        }
        private void InitInputCostControls()
        {
            InitCategoryCombo();
            InitSubCategoryCombo();

            rbTypeCard.IsChecked = true;
            txtDetail.Text = string.Empty;
            txtCost.Text = string.Empty;
        }
        private void InitCategoryCombo()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add(string.Empty);
            SettingManager.GetCategoryList().ForEach(elem => cboCategory.Items.Add(elem.Category));
            cboCategory.SelectedIndex = 0;
        }
        private void InitSubCategoryCombo()
        {
            cboSubCategory.Items.Clear();
        }

        private void DisplayCostHistory()
        {
            costHistory.Display(lastInquiryType);
        }
        private void DisplaySubCategory(string aCategory)
        {
            cboSubCategory.Items.Clear();
            cboSubCategory.Items.Add(string.Empty);
            SettingManager.GetSubCategoryList(aCategory).ForEach(elem => cboSubCategory.Items.Add(elem.SubCategory));
            cboSubCategory.SelectedIndex = 0;
        }

        private void DisplayInputCostForAdd()
        {
            lblTitle.Text = "지출 내역 입력";
            costDatePicker.Date = DateTime.Now;

            btnInputCost.Visibility = Visibility.Visible;
            btnDeleteCost.Visibility = Visibility.Collapsed;
            btnUpdateCost.Visibility = Visibility.Collapsed;
        }
        private void DisplayInputCostForUpdate(CostHistoryModel aCostHistoryModel)
        {
            lblTitle.Text = "지출 내역 수정";
            costDatePicker.Date = aCostHistoryModel.CostDateTime;
            cboCategory.SelectedValue = aCostHistoryModel.Category;
            cboSubCategory.SelectedValue = aCostHistoryModel.SubCategory;
            txtDetail.Text = aCostHistoryModel.Description;
            rbTypeCard.IsChecked = (aCostHistoryModel.CostType == "카드") ? true : false;
            rbTypeCash.IsChecked = !rbTypeCard.IsChecked;
            txtCost.Text = aCostHistoryModel.Cost.ToString();

            btnInputCost.Visibility = Visibility.Collapsed;
            btnDeleteCost.Visibility = Visibility.Visible;
            btnUpdateCost.Visibility = Visibility.Visible;

            if (inputCostMainSplitView.IsPaneOpen == false)
            {
                inputCostMainSplitView.IsPaneOpen = true;
            }
        }

        private void SaveCostInfo()
        {
            CostManager.SaveConstInfo(MakeCostInfoViaInputCostControls());
        }
        private void UpdateCostInfo()
        {
            CostInfo costInfo = MakeCostInfoViaInputCostControls();
            costInfo.Id = costHistory.SelectedItem.Id;

            CostManager.UpdateCostInfo(costInfo);
        }
        private void DeleteCostInfo()
        {
            if (costHistory.SelectedItem != null)
            {
                CostManager.DeleteCostInfo(costHistory.SelectedItem.Id);
            }
        }
        private CostInfo MakeCostInfoViaInputCostControls()
        {
            CostInfo costInfo = new CostInfo();
            costInfo.UserID = LoginInfo.UserID;
            costInfo.CostDate = costDatePicker.Date.Value.DateTime;
            costInfo.Category = cboCategory.SelectedValue.ToString();
            costInfo.SubCategory = cboSubCategory.SelectedValue.ToString();
            costInfo.CostType = (rbTypeCard.IsChecked == true) ? "카드" : "현금";
            costInfo.Cost = int.Parse(txtCost.Text.Trim());
            costInfo.Description = txtDetail.Text;

            return costInfo;
        }

        private void btnViewAddCostPanel_Click(object sender, RoutedEventArgs e)
        {
            InitInputCostControls();
            DisplayInputCostForAdd();

            if (inputCostMainSplitView.IsPaneOpen == false)
            {
                inputCostMainSplitView.IsPaneOpen = true;
            }
        }
        private void btnInputCost_Click(object sender, RoutedEventArgs e)
        {
            SaveCostInfo();
            inputCostMainSplitView.IsPaneOpen = false;
            DisplayCostHistory();
        }
        private void btnDeleteCost_Click(object sender, RoutedEventArgs e)
        {
            DeleteCostInfo();
            inputCostMainSplitView.IsPaneOpen = false;
            DisplayCostHistory();
        }
        private void btnUpdateCost_Click(object sender, RoutedEventArgs e)
        {
            UpdateCostInfo();
            inputCostMainSplitView.IsPaneOpen = false;
            DisplayCostHistory();
        }

        private void cboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cboCategory.SelectedValue == string.Empty)
            {
                InitSubCategoryCombo();
            }
            else
            {
                DisplaySubCategory((string)cboCategory.SelectedValue);
            }
        }

        private void btnSearchDay_Click(object sender, RoutedEventArgs e)
        {
            lastInquiryType = InquiryType.Today;
            costHistory.Display(lastInquiryType);
        }
        private void btnSearchWeek_Click(object sender, RoutedEventArgs e)
        {
            lastInquiryType = InquiryType.Week;
            costHistory.Display(lastInquiryType);
        }
        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {
            lastInquiryType = InquiryType.Month;
            costHistory.Display(lastInquiryType);
        }
        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            lastInquiryType = InquiryType.Year;
            costHistory.Display(lastInquiryType);
        }
        private void btnModifyCost_Click(object sender, RoutedEventArgs e)
        {
        }

        private void inputCostMainSplitView_PaneClosed(SplitView sender, object args)
        {
            InitInputCostControls();
        }
    }
}
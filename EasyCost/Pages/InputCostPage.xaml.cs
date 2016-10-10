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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
            CheckIsValidCostInfoData();
            CostManager.SaveConstInfo(MakeCostInfoUsingInputCostControls());
        }
        private void UpdateCostInfo()
        {
            CostInfo costInfo = MakeCostInfoUsingInputCostControls();
            costInfo.Id = costHistory.SelectedItem.Id;

            CheckIsValidCostInfoData();
            CostManager.UpdateCostInfo(costInfo);
        }
        private void DeleteCostInfo()
        {
            if (costHistory.SelectedItem != null)
            {
                CostManager.DeleteCostInfo(costHistory.SelectedItem.Id);
            }
        }
        private void CheckIsValidCostInfoData()
        {
            if (costDatePicker.Date.Value.DateTime > DateTime.Now)
            {
                costDatePicker.Focus(FocusState.Keyboard);
                throw new Exception("현재 시점보다 이후의 날짜는 선택할 수 없습니다");
            }
            else if (cboCategory.SelectedValue.ToString() == string.Empty)
            {
                cboCategory.Focus(FocusState.Keyboard);
                throw new Exception("카테고리 분류를 선택해 주세요");
            }
            else if (cboSubCategory.SelectedValue.ToString() == string.Empty)
            {
                cboSubCategory.Focus(FocusState.Keyboard);
                throw new Exception("세부 분류를 선택해 주세요");
            }
            else if (txtCost.Text.Trim() == string.Empty)
            {
                txtCost.Focus(FocusState.Keyboard);
                throw new Exception("지출 금액을 입력해 주세요");
            }
        }
        //private async Task<bool> IsValidCostInfoData()
        //{
        //    var dialog = new MessageDialog(string.Empty, "확인");
        //    dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });

        //    if (costDatePicker.Date.Value.DateTime > DateTime.Now)
        //    {
        //        dialog.Content = "현재 시점보다 이후의 날짜는 선택할 수 없습니다";
        //        var res = await dialog.ShowAsync();
        //        costDatePicker.Focus(FocusState.Keyboard);
        //        return false;
        //    }
        //    else if (cboCategory.SelectedValue.ToString() == string.Empty)
        //    {
        //        dialog.Content = "카테고리 분류를 선택해 주세요";
        //        var res = await dialog.ShowAsync();
        //        cboCategory.Focus(FocusState.Keyboard);
        //        return false;
        //    }
        //    else if (cboSubCategory.SelectedValue.ToString() == string.Empty)
        //    {
        //        dialog.Content = "세부 분류를 선택해 주세요";
        //        var res = await dialog.ShowAsync();
        //        cboSubCategory.Focus(FocusState.Keyboard);
        //        return false;
        //    }
        //    else if (txtCost.Text.Trim() == string.Empty)
        //    {
        //        dialog.Content = "지출 금액을 입력해 주세요";
        //        var res = await dialog.ShowAsync();
        //        txtCost.Focus(FocusState.Keyboard);
        //        return false;
        //    }

        //    return true;
        //}
        private CostInfo MakeCostInfoUsingInputCostControls()
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
        private async void btnInputCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveCostInfo();
                inputCostMainSplitView.IsPaneOpen = false;
                DisplayCostHistory();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                await dialog.ShowAsync();
            }
        }
        private async void btnUpdateCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateCostInfo();
                inputCostMainSplitView.IsPaneOpen = false;
                DisplayCostHistory();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }
        private async void btnDeleteCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteCostInfo();
                inputCostMainSplitView.IsPaneOpen = false;
                DisplayCostHistory();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "확인");
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
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

        private void inputCostMainSplitView_PaneClosed(SplitView sender, object args)
        {
            InitInputCostControls();
        }

        private void txtCost_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            double dtemp;
            if (!double.TryParse(sender.Text, out dtemp) && sender.Text != "")
            {
                int pos = sender.SelectionStart - 1;
                sender.Text = sender.Text.Remove(pos, 1);
                sender.SelectionStart = pos;
            }
        }

        private async void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            if (costHistory.CostInfo.Count != 0)
            {
                await CostManager.ExportToExcel(costHistory.CostInfo);
            }
        }
    }
}
using EasyCost.Bases.Login;
using EasyCost.Databases.TableModels;
using EasyCost.DataModels;
using EasyCost.Helpers;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class InputCostPage : Page
    {
        InquiryType _lastInquiryType;
        Button _currentButton = null;
        List<Button> _searchButtonList = new List<Button>();
        public InputCostPage()
        {
            this.InitializeComponent();

            costHistory.CostItemSelectedEvent += new Controls.ViewCostHistoryControl.CostItemSelected(DisplayInputCostForUpdate);
            dateSearchControl.DateSelectedEvent += () => Flyout.ShowAttachedFlyout(btnSearchCustom);
            dateSearchControl.DisplayDateEvent += (x, y) => DisplayCustomCostHistory(x, y);
            Flyout.SetAttachedFlyout(btnSearchCustom, searchCustomFlyout);

            InitControls();
            DisplayCostHistory();
        }

        private void InitControls()
        {
            _searchButtonList.Clear();
            _searchButtonList.Add(btnSearchDay);
            _searchButtonList.Add(btnSearchWeek);
            _searchButtonList.Add(btnSearchMonth);
            _searchButtonList.Add(btnSearchYear);
            _searchButtonList.Add(btnSearchAll);
            _searchButtonList.Add(btnSearchCustom);

            _lastInquiryType = InquiryType.Today;
            _currentButton = btnSearchDay;
            SetSearchButtonColor(_currentButton);

            InitInputCostControls(GetCurrentCategoryType());
        }
        private void InitInputCostControls(string aCategoryType)
        {
            InitCategoryCombo(aCategoryType);
            InitSubCategoryCombo();

            rbTypeCard.IsChecked = true;
            txtDetail.Text = string.Empty;
            txtCost.Text = string.Empty;
        }
        private void InitInputCostControlsForContinue()
        {
            cboCategory.SelectedIndex = 0;
            txtDetail.Text = string.Empty;
            txtCost.Text = string.Empty;

            cboCategory.Focus(FocusState.Keyboard);
        }
        private void InitCategoryCombo(string aCategoryType)
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add(string.Empty);
            SettingManager.GetCategoryList(aCategoryType).ForEach(elem => cboCategory.Items.Add(elem.Category));
            cboCategory.SelectedIndex = 0;
        }
        private void InitSubCategoryCombo()
        {
            cboSubCategory.Items.Clear();
        }

        private void DisplayCostHistory()
        {
            costHistory.Display(_lastInquiryType);
        }
        private void DisplayCustomCostHistory(DateTime aFromDate, DateTime aToDate)
        {
            _currentButton = btnSearchCustom;
            SetSearchButtonColor(_currentButton);

            costHistory.Display(aFromDate, aToDate);
        }


        private void DisplaySubCategory(string aCategoryType, string aCategory)
        {
            cboSubCategory.Items.Clear();
            cboSubCategory.Items.Add(string.Empty);
            SettingManager.GetSubCategoryList(aCategoryType, aCategory).ForEach(elem => cboSubCategory.Items.Add(elem.SubCategory));
            cboSubCategory.SelectedIndex = 0;
        }

        private void DisplayInputCostForAdd(string aCategoryType)
        {
            if (aCategoryType == CategoryType.Expense)
            {
                lblTitle.Text = "지출 내역 입력";
                lblDetail.Text = "지출 내역";

                rbTypeCard.Visibility = Visibility.Visible;
            }
            else
            {
                lblTitle.Text = "수입 내역 입력";
                lblDetail.Text = "수입 내역";

                rbTypeCard.Visibility = Visibility.Collapsed;
                rbTypeCash.IsChecked = true;
            }
            
            costDatePicker.Date = DateTime.Now;

            btnInputCost.Visibility = Visibility.Visible;
            btnInputCostContinue.Visibility = Visibility.Visible;
            btnDeleteCost.Visibility = Visibility.Collapsed;
            btnUpdateCost.Visibility = Visibility.Collapsed;
        }
        private void DisplayInputCostForUpdate(CostHistoryModel aCostHistoryModel)
        {
            InitCategoryCombo(aCostHistoryModel.CategoryType);

            if (aCostHistoryModel.CategoryType == CategoryType.Expense)
            {
                lblTitle.Text = "지출 내역 수정";
                lblDetail.Text = "지출 내역";

                rbTypeCard.Visibility = Visibility.Visible;
                rbTypeCash.IsChecked = (aCostHistoryModel.CostType == "현금") ? true : false;
            }
            else
            {
                lblTitle.Text = "수입 내역 수정";
                lblDetail.Text = "수입 내역";

                rbTypeCard.Visibility = Visibility.Collapsed;
                rbTypeCash.IsChecked = true;
            }

            costDatePicker.Date = aCostHistoryModel.CostDateTime;
            cboCategory.SelectedValue = aCostHistoryModel.Category;
            cboSubCategory.SelectedValue = aCostHistoryModel.SubCategory;
            txtDetail.Text = aCostHistoryModel.Description;
            txtCost.Text = aCostHistoryModel.Cost.ToString();

            btnInputCost.Visibility = Visibility.Collapsed;
            btnInputCostContinue.Visibility = Visibility.Collapsed;
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
        private CostInfo MakeCostInfoUsingInputCostControls()
        {
            CostInfo costInfo = new CostInfo();
            costInfo.UserID = LoginInfo.UserID;
            costInfo.CostDate = costDatePicker.Date.Value.DateTime;
            costInfo.CategoryType = GetCurrentCategoryType();
            costInfo.Category = cboCategory.SelectedValue.ToString();
            costInfo.SubCategory = cboSubCategory.SelectedValue.ToString();
            costInfo.CostType = (rbTypeCard.IsChecked == true) ? "카드" : "현금";
            costInfo.Cost = int.Parse(txtCost.Text.Trim());
            costInfo.Description = txtDetail.Text;

            return costInfo;
        }

        private string GetCurrentCategoryType()
        {
            return (lblTitle.Text.StartsWith("지출") ? CategoryType.Expense : CategoryType.Income);
        }

        private void btnViewIncomePanel_Click(object sender, RoutedEventArgs e)
        {
            InitInputCostControls(CategoryType.Income);
            DisplayInputCostForAdd(CategoryType.Income);

            if (inputCostMainSplitView.IsPaneOpen == false)
            {
                inputCostMainSplitView.IsPaneOpen = true;
            }
        }

        private void btnViewExpensePanel_Click(object sender, RoutedEventArgs e)
        {
            InitInputCostControls(CategoryType.Expense);
            DisplayInputCostForAdd(CategoryType.Expense);

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

        private async void btnInputCostContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveCostInfo();
                DisplayCostHistory();
                inputCostMainSplitView.IsPaneOpen = true;
                InitInputCostControlsForContinue();
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
                string categoryType = (lblTitle.Text.StartsWith("지출")) ? CategoryType.Expense : CategoryType.Income;
                DisplaySubCategory(categoryType, (string)cboCategory.SelectedValue);
            }
        }

        private void btnSearchDay_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            _lastInquiryType = InquiryType.Today;
            costHistory.Display(_lastInquiryType);
        }
        private void btnSearchWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            _lastInquiryType = InquiryType.Week;
            costHistory.Display(_lastInquiryType);
        }
        private void btnSearchMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            _lastInquiryType = InquiryType.Month;
            costHistory.Display(_lastInquiryType);
        }
        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            _lastInquiryType = InquiryType.Year;
            costHistory.Display(_lastInquiryType);
        }
        private void btnSearchAll_Click(object sender, RoutedEventArgs e)
        {
            _currentButton = (Button)sender;
            SetSearchButtonColor(_currentButton);

            _lastInquiryType = InquiryType.All;
            costHistory.Display(_lastInquiryType);
        }

        private void inputCostMainSplitView_PaneClosed(SplitView sender, object args)
        {
            InitInputCostControls(GetCurrentCategoryType());
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
        
        private void btnSearchButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetSearchButtonColor((Button)sender);
        }

        private void btnSearchButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SetSearchButtonColor(_currentButton);
        }

        private void SetSearchButtonColor(Button aMouseOverButton)
        {
            const string main_color = "006FB6";

            foreach (var button in _searchButtonList)
            {
                if (button == _currentButton || button == aMouseOverButton)
                {
                    //button.Background = MyColorHelper.GetSolidColorBrush(main_color);
                    button.Background = new SolidColorBrush(Windows.UI.Colors.Red);
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
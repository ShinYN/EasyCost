using EasyCost.Databases;
using EasyCost.Databases.TableModels;
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
        public InputCostPage()
        {
            this.InitializeComponent();
            InitControls();
            DisplayCostHistory();
        }

        private void InitControls()
        {
            InitInputCostControls();
            InitViewCostHistoryControls();            
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

        }
        private void InitSubCategoryCombo()
        {

        }


        private void InitViewCostHistoryControls()
        {

        }

        private void DisplayCostHistory()
        {
            costHistory.Display();
        }

        private void btnInputCost_Click(object sender, RoutedEventArgs e)
        {
            CostInfo costInfo = new CostInfo();
            costInfo.CostDate = DateTime.Now.ToString("yyyyMMdd");
            costInfo.Category = cboCategory.SelectedValue.ToString();
            costInfo.SubCategory = cboSubCategory.SelectedValue.ToString();
            costInfo.CostType = (rbTypeCard.IsChecked == true) ? "Card" : "Cash";
            costInfo.Cost = int.Parse(txtCost.Text.Trim());
            costInfo.Description = txtDetail.Text;

            DBConnHandler.Cost.SaveConstInfo(costInfo);

            DisplayCostHistory();
        }
    }
}

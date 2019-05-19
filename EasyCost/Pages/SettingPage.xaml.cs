using EasyCost.Pages.Settings;
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
    public sealed partial class SettingPage : Page
    {
        private const string MENU_INCOME_CATEGORY = "수입 카테고리 관리";
        private const string MENU_EXPENSE_CATEGORY = "지출 카테고리 관리";
        private const string MENU_CARDMANAGEMENT = "카드 정보 관리";
        private const string MENU_PROGRAMINFO = "프로그램 정보";
        private const string MENU_DATASYNC = "데이터 관리";
        private bool mIsIncomePage = false;

        public SettingPage()
        {
            this.InitializeComponent();
            InitSettingItems();
            settingFrame.Navigate(typeof(CategorySettingPage), CategoryType.Expense);
            mIsIncomePage = false;
        }

        private void InitSettingItems()
        {
            lsvSettingItem.Items.Clear();
            lsvSettingItem.Items.Add(new { Item = MENU_EXPENSE_CATEGORY });
            lsvSettingItem.Items.Add(new { Item = MENU_INCOME_CATEGORY });
            lsvSettingItem.Items.Add(new { Item = MENU_CARDMANAGEMENT });
            lsvSettingItem.Items.Add(new { Item = MENU_DATASYNC });
            lsvSettingItem.Items.Add(new { Item = MENU_PROGRAMINFO });
            lsvSettingItem.SelectedIndex = 0;
        }

        private void DisplaySettingPage(string aSelectedItemString)
        {
            if (aSelectedItemString == MENU_INCOME_CATEGORY)
            {
                if ((settingFrame.CurrentSourcePageType != typeof(CategorySettingPage)) || (mIsIncomePage == false))
                {
                    settingFrame.Navigate(typeof(CategorySettingPage), CategoryType.Income);
                    mIsIncomePage = true;
                }
            }
            else if (aSelectedItemString == MENU_EXPENSE_CATEGORY)
            {
                if ((settingFrame.CurrentSourcePageType != typeof(CategorySettingPage)) || (mIsIncomePage))
                {
                    settingFrame.Navigate(typeof(CategorySettingPage), CategoryType.Expense);
                    mIsIncomePage = false;
                }
            }
            else if (aSelectedItemString == MENU_CARDMANAGEMENT)
            {
                if (settingFrame.CurrentSourcePageType != typeof(CardManagementPage))
                {
                    settingFrame.Navigate(typeof(CardManagementPage));
                }
            }
            else if (aSelectedItemString == MENU_PROGRAMINFO)
            {
                if (settingFrame.CurrentSourcePageType != typeof(ProgramInfoPage))
                {
                    settingFrame.Navigate(typeof(ProgramInfoPage));
                }
            }
            else if (aSelectedItemString == MENU_DATASYNC)
            {
                if (settingFrame.CurrentSourcePageType != typeof(DataSyncPage))
                {
                    settingFrame.Navigate(typeof(DataSyncPage));
                }
            }
        }

        private void lsvSettingItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(TextBlock))
            {
                return;
            }

            DisplaySettingPage(((TextBlock)e.OriginalSource).Text);
        }

        private void lsvSettingItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                dynamic clickedItem = e.ClickedItem;
                DisplaySettingPage(clickedItem.Item);
            }
        }
    }
}

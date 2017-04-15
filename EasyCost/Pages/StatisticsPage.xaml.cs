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
using Windows.UI.Xaml.Media;
using EasyCost.Pages.Statistics;
using Windows.UI.Xaml.Input;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        private const string MENU_EXPENSE_STATISTICS = "지출 통계 정보";
        private const string MENU_INCOME_STATISTICS = "수입 통계 정보";
        private bool mIsIncomePage = false;

        public StatisticsPage()
        {
            this.InitializeComponent();
            InitSettingItems();
            settingFrame.Navigate(typeof(ViewStatisticsPage), CategoryType.Expense);
            mIsIncomePage = false;
        }

        private void InitSettingItems()
        {
            lsvSettingItem.Items.Clear();
            lsvSettingItem.Items.Add(new { Item = MENU_EXPENSE_STATISTICS });
            lsvSettingItem.Items.Add(new { Item = MENU_INCOME_STATISTICS });
            lsvSettingItem.SelectedIndex = 0;
        }

        private void DisplayStatisticsPage(string aSelectedItemString)
        {
            if (aSelectedItemString == MENU_INCOME_STATISTICS)
            {
                if ((settingFrame.CurrentSourcePageType != typeof(ViewStatisticsPage)) || (mIsIncomePage == false))
                {
                    settingFrame.Navigate(typeof(ViewStatisticsPage), CategoryType.Income);
                    mIsIncomePage = true;
                }
            }
            else if (aSelectedItemString == MENU_EXPENSE_STATISTICS)
            {
                if ((settingFrame.CurrentSourcePageType != typeof(ViewStatisticsPage)) || (mIsIncomePage))
                {
                    settingFrame.Navigate(typeof(ViewStatisticsPage), CategoryType.Expense);
                    mIsIncomePage = false;
                }
            }
        }

        private void lsvSettingItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(TextBlock))
            {
                return;
            }

            DisplayStatisticsPage(((TextBlock)e.OriginalSource).Text);
        }

        private void lsvSettingItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                dynamic clickedItem = e.ClickedItem;
                DisplayStatisticsPage(clickedItem.Item);
            }
        }
    }
}

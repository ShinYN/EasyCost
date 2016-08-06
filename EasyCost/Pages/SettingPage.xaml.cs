using EasyCost.Pages.Settings;
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
        private const string MENU_CATEGORY = "카테고리 관리";
        private const string MENU_PROGRAMINFO = "프로그램 정보";
        public SettingPage()
        {
            this.InitializeComponent();
            InitSettingItems();
            settingFrame.Navigate(typeof(CategorySettingPage));
        }

        private void InitSettingItems()
        {
            lsvSettingItem.Items.Clear();
            lsvSettingItem.Items.Add(new { Item = MENU_CATEGORY });
            lsvSettingItem.Items.Add(new { Item = MENU_PROGRAMINFO });
            lsvSettingItem.SelectedIndex = 0;
        }

        private void DisplaySettingPage(string aSelectedItemString)
        {
            if (aSelectedItemString == MENU_CATEGORY)
            {
                if (settingFrame.CurrentSourcePageType != typeof(CategorySettingPage))
                {
                    settingFrame.Navigate(typeof(CategorySettingPage));
                }
            }
            else if (aSelectedItemString == MENU_PROGRAMINFO)
            {
                if (settingFrame.CurrentSourcePageType != typeof(ProgramInfoPage))
                {
                    settingFrame.Navigate(typeof(ProgramInfoPage));
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
    }
}

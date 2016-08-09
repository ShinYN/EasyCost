using EasyCost.Pages;
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

// 빈 페이지 항목 템플릿은 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 에 문서화되어 있습니다.

namespace EasyCost
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(InputCostPage));

            InitMenu();
        }


        private void InitMenu()
        {
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuWriteCost.png", menuText = "지출 내역 관리" });
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuStatistics.png", menuText = "지출 통계 보기" });
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuSetting.png", menuText = "프로그램 설정" });
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CurrentSourcePageType != typeof(InputCostPage))
            {
                mainFrame.Navigate(typeof(InputCostPage));
            }
        }

        private void btnInquiry_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CurrentSourcePageType != typeof(StatisticsPage))
            {
                mainFrame.Navigate(typeof(StatisticsPage));
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CurrentSourcePageType != typeof(SettingPage))
            {
                mainFrame.Navigate(typeof(SettingPage));
            }
        }

        private void btnMenuFolder_Click(object sender, RoutedEventArgs e)
        {
            menuSplitView.IsPaneOpen = !menuSplitView.IsPaneOpen;
        }
    }
}

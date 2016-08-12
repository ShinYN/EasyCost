using EasyCost.DataModels;
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
        private const string MENU_WRITECOST = "지출 내역 관리";
        private const string MENU_STATISTICS = "지출 통계 보기";
        private const string MENU_PROGRAMINFO = "프로그램 정보";
        private const string MENU_SETTINGS = "프로그램 설정";

        public MainPage()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(InputCostPage));

            InitializeMenu();
        }

        private void InitializeMenu()
        {
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuWriteCost_Dark.png", menuText = "지출 내역 관리" });
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuStatistics_Dark.png", menuText = "지출 통계 보기" });
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuSetting_Dark.png", menuText = "프로그램 설정" });
            menuList.Items.Add(new { ImagePath = "ms-appx:///Assets/MenuIcons/menuProgramInfo.png", menuText = "프로그램 정보" });
        }

        private void DisplayMenuFromText(string aMenuText)
        {
            if (aMenuText == MENU_WRITECOST)
            {
                if (mainFrame.CurrentSourcePageType != typeof(InputCostPage))
                {
                    mainFrame.Navigate(typeof(InputCostPage));
                }
            }
            else if (aMenuText == MENU_STATISTICS)
            {
                if (mainFrame.CurrentSourcePageType != typeof(StatisticsPage))
                {
                    mainFrame.Navigate(typeof(StatisticsPage));
                }
            }
            else if (aMenuText == MENU_SETTINGS)
            {
                if (mainFrame.CurrentSourcePageType != typeof(SettingPage))
                {
                    mainFrame.Navigate(typeof(SettingPage));
                }
            }
            else if (aMenuText == MENU_PROGRAMINFO)
            {

            }
        }
        private void DisplayMenuFromImage(Image aMenuImage)
        {
            //if (aMenuImage)
            //{
            //    if (mainFrame.CurrentSourcePageType != typeof(InputCostPage))
            //    {
            //        mainFrame.Navigate(typeof(InputCostPage));
            //    }
            //}
            //else if (aMenuText == MENU_STATISTICS)
            //{
            //    if (mainFrame.CurrentSourcePageType != typeof(StatisticsPage))
            //    {
            //        mainFrame.Navigate(typeof(StatisticsPage));
            //    }
            //}
            //else if (aMenuText == MENU_SETTINGS)
            //{
            //    if (mainFrame.CurrentSourcePageType != typeof(SettingPage))
            //    {
            //        mainFrame.Navigate(typeof(SettingPage));
            //    }
            //}
            //else if (aMenuText == MENU_PROGRAMINFO)
            //{

            //}
        }

        private void btnMenuFolder_Click(object sender, RoutedEventArgs e)
        {
            menuSplitView.IsPaneOpen = !menuSplitView.IsPaneOpen;
        }
        private void menuList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                DisplayMenuFromText(((TextBlock)e.OriginalSource).Text);
            }
            else if (e.OriginalSource.GetType() == typeof(Image))
            {
               // DisplayMenuByImage()
            }
            else
            {
                return;
            }
        }
    }
}

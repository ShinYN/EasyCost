using EasyCost.Databases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace EasyCost.Pages.Settings
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class DataSyncPage : Page
    {
        public DataSyncPage()
        {
            this.InitializeComponent();
        }

        private async void btnInitData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("정말로 데이터를 모두 초기화 하시겠습니까?.\r\n초기화 된 데이터는 다시 복구할 수 없습니다.");
            dialog.Title = "확인";
            dialog.Commands.Add(new UICommand { Label = "예", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "아니요", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                InitData();
            }
        }

        private void InitData()
        {
            DBConnHandler.DropDB();
            DBConnHandler.CreateDB();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(StartPage));
        }
    }
}

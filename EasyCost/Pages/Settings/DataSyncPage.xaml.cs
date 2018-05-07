using EasyCost.Databases;
using EasyCost.Helpers;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages.Settings
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class DataSyncPage : Page
    {
        private string BACKUP_LOCATION_GOOGLE = "Google Drive";

        public DataSyncPage()
        {
            this.InitializeComponent();
        }

        private void InitDataBackupControls()
        {
            cboDataSyncFrom.Items.Clear();
            cboDataSyncFrom.Items.Add(string.Empty);
            cboDataSyncFrom.Items.Add(BACKUP_LOCATION_GOOGLE);
            cboDataSyncFrom.SelectedIndex = 0;
            
            cboDataSyncTo.Items.Clear();
            cboDataSyncTo.Items.Add(string.Empty);
            cboDataSyncTo.Items.Add(BACKUP_LOCATION_GOOGLE);
            cboDataSyncTo.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitDataBackupControls();
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

        private async void btnBackupData_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = cboDataSyncFrom.SelectedValue.ToString();
            if (string.IsNullOrEmpty(selectedItem))
            {
                return;
            }

            var dialog = new MessageDialog($"{selectedItem}으로 현재 데이터를 백업하시겠습니까?");
            dialog.Title = "확인";
            dialog.Commands.Add(new UICommand { Label = "예", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "아니요", Id = 1 });

            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                var result = await GoogleDriveHelper.SaveFileAsync();
                var resultMessage = string.Empty;
                if (result == System.Net.HttpStatusCode.OK)
                {
                    resultMessage = "정상적으로 백업되었습니다";
                }
                else if (result == System.Net.HttpStatusCode.NoContent)
                {
                    resultMessage = "백업 중 오류가 발생했습니다. 백업할 파일을 찾지 못했습니다";
                }
                else
                {
                    resultMessage = "백업 중 오류가 발생했습니다. 다시 시도 부탁 드립니다";
                }

                dialog = new MessageDialog(resultMessage);
                dialog.Title = "백업 결과";
                dialog.Commands.Add(new UICommand { Label = "예", Id = 0 });
                await dialog.ShowAsync();
            }
        }

        private async void btnResotreData_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = cboDataSyncTo.SelectedValue.ToString();
            if (string.IsNullOrEmpty(selectedItem))
            {
                return;
            }

            var dialog = new MessageDialog($"{selectedItem}에서 저장된 데이터를 가져오시겠습니까?");
            dialog.Title = "확인";
            dialog.Commands.Add(new UICommand { Label = "예", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "아니요", Id = 1 });

            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                var fileData = await GoogleDriveHelper.LoadFileAsync();
            }
        }
    }
}

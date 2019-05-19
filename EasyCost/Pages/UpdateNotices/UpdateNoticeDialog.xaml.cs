using EasyCost.Databases;
using EasyCost.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyCost.Pages.UpdateNotices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateNoticeDialog : ContentDialog
    {
        public UpdateNoticeDialog()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string packageVersion = DBConnHandler.GetPackageVersion();
            UpdateNoticeManager.CheckNotice(packageVersion);

            this.Hide();
        }
    }
}

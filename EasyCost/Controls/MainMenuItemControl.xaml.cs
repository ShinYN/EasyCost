using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EasyCost.Controls
{
    public sealed partial class MainMenuItemControl : UserControl
    {
        public delegate void MenuItemClickDelegate();
        public event MenuItemClickDelegate MenuItemClick;
        public MainMenuItemControl()
        {
            this.InitializeComponent();
        }

        public string MenuName
        {
            get { return menuName.Text; }
            set { menuName.Text = value; }
        }
        public string MenuDescription
        {
            get { return menuDescription.Text; }
            set { menuDescription.Text = value; }
        }
        public ImageSource ImageUri
        {
            get { return menuImage.Source; }
            set { menuImage.Source = value; }
        }

        private void menuImageBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuItemClick();
        }
    }
}

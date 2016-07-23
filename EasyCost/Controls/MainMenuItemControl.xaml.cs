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

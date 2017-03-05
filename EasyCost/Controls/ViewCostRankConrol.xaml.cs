using EasyCost.DataModels;
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
    public sealed partial class ViewCostRankConrol : UserControl
    {
        public delegate void ItemSelected(CategoryCostModel costModel);
        public event ItemSelected ItemSelectedEvent;
        public ViewCostRankConrol()
        {
            this.InitializeComponent();
        }

        public string Title
        {
            get { return txtTitle.Text.Trim(); }
            set { txtTitle.Text = value; }
        }

        public void Clear()
        {
            Title = string.Empty;
            lsvHistory.Items.Clear();
        }
        public void Display(List<CategoryCostModel> aCategoryCostList)
        {
            lsvHistory.Items.Clear();

            if (aCategoryCostList == null)
            {
                return;
            }

            foreach (var item in aCategoryCostList)
            {
                lsvHistory.Items.Add(item);
            }
        }

        private void lsvHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null && e.ClickedItem.GetType() == typeof(CategoryCostModel))
            {
                ItemSelectedEvent((CategoryCostModel)e.ClickedItem);
            }
        }
    }
}

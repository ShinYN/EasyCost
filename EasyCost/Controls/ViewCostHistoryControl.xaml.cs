using EasyCost.Databases;
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
    public sealed partial class ViewCostHistoryControl : UserControl
    {
        public ViewCostHistoryControl()
        {
            this.InitializeComponent();
        }

        public void Display()
        {
            lsvHistory.Items.Clear();

            int totalCost = 0;
            DBConnHandler.Cost.GetCostInfo().ForEach(elem =>
                {
                    lsvHistory.Items.Add(new
                    {
                        Id = elem.Id,
                        CostDate = elem.CostDate,
                        Category = elem.Category,
                        SubCategory = elem.SubCategory,
                        CostType = elem.CostType,
                        Cost = elem.Cost,
                        Description = elem.Description
                    });

                    totalCost += elem.Cost;
                }
            );

            lblTotalCost.Text = totalCost.ToString();
        }
    }
}

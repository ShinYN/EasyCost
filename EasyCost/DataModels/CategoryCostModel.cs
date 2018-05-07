using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.DataModels
{
    public class CategoryCostModel
    {
        public int Index { get; set; } = 0;
        public string CategoryType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int ItemCount { get; set; } = 0;
        public double CostRatio { get; set; } = 0;
        public int Cost { get; set; } = 0; 
        public string CostString
        {
            get
            {
                return Cost.ToString("#,##0") + "원";
            }
        }
        public string CostRatioString
        {
            get
            {
                return CostRatio.ToString("N2") + " %";
            }
        }
        public string CategoryWithCount
        {
            get
            {
                if (ItemCount == 0)
                {
                    return Category;
                }
                else
                {
                    return $"{Category}({ItemCount})";
                }
            }
        }
    }
}

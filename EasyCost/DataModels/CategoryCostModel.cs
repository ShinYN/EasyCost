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
        public string Category { get; set; } = string.Empty;
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
    }
}

using EasyCost.Databases.TableModels;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.DataModels
{
    public class CostStatisticsModel
    {
        public InquiryType InquiryType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DisplayString { get; set; } = string.Empty;
        public int Cost
        {
            get
            {
                return CostInfo.Sum(elem => elem.Cost);
            }
        }
        public int CardCost
        {
            get
            {
                return CostInfo.Where(elem => elem.CostType == "카드").Sum(elem => elem.Cost);
            }
        }
        public int CashCost
        {
            get
            {
                return CostInfo.Where(elem => elem.CostType == "현금").Sum(elem => elem.Cost);
            }
        }

        public string CostString
        {
            get
            {
                return Cost.ToString("#,##0");
            }
        }
        public string CardCostString
        {
            get
            {
                return CardCost.ToString("#,##0");
            }
        }
        public string CashCostString
        {
            get
            {
                return CashCost.ToString("#,##0");
            }
        }


        public List<CostInfo> CostInfo { get; set; } = new List<CostInfo>();
    }
}

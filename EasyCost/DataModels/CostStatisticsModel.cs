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
    public class CostStatisticsModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
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
        public List<CostInfo> CostInfo { get; set; } = new List<CostInfo>();
    }
}

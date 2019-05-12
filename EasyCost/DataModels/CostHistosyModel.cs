using System;
using System.ComponentModel;

namespace EasyCost.DataModels
{
    public class CostHistoryModel : INotifyPropertyChanged
    {       
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; RaiseProperty(nameof(Selected)); }
        }

        /// <summary>
        /// Grid에서 표현되는 순서 값
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// CostInfo에서 Key ID 값
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 지출 날짜.(yyyy/MM/dd)
        /// </summary>
        public string CostDate { get; set; }
        /// <summary>
        /// 지출 날짜. Datetime
        /// </summary>
        public DateTime CostDateTime { get; set; }
        /// <summary>
        /// 지출, 수비 분류
        /// </summary>
        public string CategoryType { get; set; }
        /// <summary>
        /// 지출, 수비 분류
        /// </summary>
        public string CategoryTypeString { get; set; }
        /// <summary>
        /// 소비 대 분류
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 소비 소 분류
        /// </summary>
        public string SubCategory { get; set; }
        /// <summary>
        /// 지출 타입. 카드 또는 현금
        /// </summary>
        public string CostType { get; set; }
        /// <summary>
        /// 지출 금액
        /// </summary>
        public int Cost { get; set; }
        /// 지출 금액 (표기용)
        /// </summary>
        public string CostString { get; set; }
        /// <summary>
        /// 세부 설명
        /// </summary>
        public string Description { get; set; }
        public string Percentage { get; set; }
    }
}

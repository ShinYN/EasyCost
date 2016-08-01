using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases.TableModels
{
    public class CostInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 지출 날짜. YYYY-MM-DD형식
        /// </summary>
        public string CostDate { get; set; }
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
        /// <summary>
        /// 세부 설명
        /// </summary>
        public string Description { get; set; }
    }
}

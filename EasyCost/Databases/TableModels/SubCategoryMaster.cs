using EasyCost.Types;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases.TableModels
{
    public class SubCategoryMaster
    {
        [PrimaryKey]
        public string CategoryType { get; set; } = string.Empty;
        [PrimaryKey]
        public string Category { get; set; } = string.Empty;
        [PrimaryKey]
        public string SubCategory { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// N : False
        /// Y : True
        /// </summary>
        public string RepeatYN { get; set; } = "N";
        /// <summary>
        /// 반복 주기
        /// </summary>
        public RepeatPeriodType RepeatPeriod { get; set; } = RepeatPeriodType.None;
        /// <summary>
        /// 반복 날짜
        /// </summary>
        public string RepeatDate { get; set; } = string.Empty;
        /// <summary>
        /// 반복 시 사용 값
        /// </summary>
        public int RepeatValue { get; set; } = 0;
    }
}

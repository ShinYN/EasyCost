using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases.TableModels
{
    public class UserMaster
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        [PrimaryKey]
        public string UserID { get; set; }
        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 로그인 타입
        /// 예: 오프라인, 구글
        /// </summary>
        public string LoginType { get; set; }
    }
}

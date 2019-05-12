using SQLite;

namespace EasyCost.Databases.TableModels
{
    public class CardMaster
    {
        /// <summary>
        /// 카드 이름
        /// </summary>
        [PrimaryKey]
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// 카드 타입 (체크카드, 신용카드)
        /// </summary>
        public string CardType { get; set; } = string.Empty;

        /// <summary>
        /// 카드 회사
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// 카드 번호
        /// </summary>
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// 설명
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}

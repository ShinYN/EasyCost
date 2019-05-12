using SQLite;

namespace EasyCost.Databases.TableModels
{
    public class CategoryMaster
    {
        [PrimaryKey]
        public string CategoryType { get; set; } = string.Empty;
        [PrimaryKey]
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

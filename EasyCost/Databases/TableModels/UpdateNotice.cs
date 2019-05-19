using SQLite;

namespace EasyCost.Databases.TableModels
{
    public class UpdateNotice
    {
        [PrimaryKey]
        public string Version { get; set; } = string.Empty;
        public bool Checked { get; set; } = false;
    }
}

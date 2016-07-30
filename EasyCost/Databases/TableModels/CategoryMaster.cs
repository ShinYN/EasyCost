using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases.TableModels
{
    public class CategoryMaster
    {
        [PrimaryKey]
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

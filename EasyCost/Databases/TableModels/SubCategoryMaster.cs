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
        public string Category { get; set; } = string.Empty;
        [PrimaryKey]
        public string SubCategory { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}

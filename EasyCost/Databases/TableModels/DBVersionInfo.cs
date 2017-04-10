using EasyCost.Types;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases.TableModels
{
    public class DBVersionInfo
    {
        [PrimaryKey]
        public DBVersionType Version { get; set; }
    }
}

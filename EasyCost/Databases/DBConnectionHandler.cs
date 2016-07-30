using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases
{
    public static class DBConnectionHandler
    {
        public static SQLiteConnection DbConnection
        {
            get
            {
                string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
                return new SQLiteConnection(dbPath);
            }
        }
    }
}

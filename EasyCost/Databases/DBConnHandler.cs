using EasyCost.Bases.Login;
using EasyCost.Databases.TableModels;
using EasyCost.Types;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Databases
{
    public static class DBConnHandler
    {
        public static SQLiteConnection DbConnection
        {
            get
            {
                string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
                return new SQLiteConnection(dbPath);
            }
        }

        public static void CreateDB()
        {
            DbConnection.CreateTable<UserMaster>();
            DbConnection.CreateTable<CostInfo>();
            DbConnection.CreateTable<CategoryMaster>();
            DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS SubCategoryMaster (Category VARCHAR(50), SubCategory VARCHAR(50), Description VARCHAR(100), PRIMARY KEY (Category, SubCategory))");
        }
        public static void DropDB()
        {
            DbConnection.DropTable<UserMaster>();
            DbConnection.DropTable<CategoryMaster>();
            DbConnection.DropTable<SubCategoryMaster>();
            DbConnection.DropTable<CostInfo>();
        }
        public static bool IsFirstConnection()
        {
            try
            {
                DbConnection.Execute(@"SELECT * FROM UserMaster");
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}

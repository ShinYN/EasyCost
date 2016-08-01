using EasyCost.Databases.TableModels;
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
        public static void Initialize()
        {
            DbConnection.CreateTable<CostInfo>();
            DbConnection.CreateTable<CategoryMaster>();
            DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS SubCategoryMaster (Category VARCHAR(50), SubCategory VARCHAR(50), Description VARCHAR(100), PRIMARY KEY (Category, SubCategory))");
        }

        private static SQLiteConnection DbConnection
        {
            get
            {
                string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
                return new SQLiteConnection(dbPath);
            }
        }

        public static class Cost
        {
            public static List<CostInfo> GetCostInfo()
            {
                return (from c in DbConnection.Table<CostInfo>()
                        select c).ToList();
            }

            public static void SaveConstInfo(CostInfo aCostInfo)
            {
                DbConnection.Insert(aCostInfo);
            }

            public static void DeleteCostInfo(CostInfo aCostInfo)
            {
                DbConnection.Delete(aCostInfo);
            }
        }

        public static class Setting
        {
            
        }
    }
}

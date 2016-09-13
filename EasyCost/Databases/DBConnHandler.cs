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

        public static bool IsFirstConnection
        {
            get
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

        public static void CreateDB()
        {
            DbConnection.CreateTable<UserMaster>();
            DbConnection.CreateTable<CostInfo>();
            DbConnection.CreateTable<CategoryMaster>();
            DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS SubCategoryMaster (Category VARCHAR(50), SubCategory VARCHAR(50), Description VARCHAR(100), PRIMARY KEY (Category, SubCategory))");

            CreateInitialCategory();
        }
        public static void DropDB()
        {
            DbConnection.DropTable<UserMaster>();
            DbConnection.DropTable<CategoryMaster>();
            DbConnection.DropTable<SubCategoryMaster>();
            DbConnection.DropTable<CostInfo>();
        }

        private static void CreateInitialCategory()
        {
            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT '교통비', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '교통비', '지하철', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '교통비', '버스', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '교통비', '택시', ''");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT '식비', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '식비', '아침', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '식비', '점심', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '식비', '저녁', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '식비', '커피', ''");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT '공과금', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '공과금', '전기요금', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '공과금', '가스요금', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '공과금', '수도요금', ''");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT '취미', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT '취미', '책', ''");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT '기타', ''");
        }
    }
}

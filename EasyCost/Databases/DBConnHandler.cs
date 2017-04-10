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
        private static DBVersionType LATEST_DB_VERSION = DBVersionType.Version2;
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
            if (IsFirstConnection)
            {
                DbConnection.CreateTable<DBVersionInfo>();
                DbConnection.CreateTable<UserMaster>();
                DbConnection.CreateTable<CostInfo>();
                DbConnection.CreateTable<CategoryMaster>();
                DbConnection.CreateTable<SubCategoryMaster>();

                CreateInitialCategory();
            }
            else
            {
                DBVersionType userDBVersion = GetUserDBVersion();
                if (userDBVersion == LATEST_DB_VERSION)
                {
                    return;
                }
                else
                {
                    MigrationDBData(userDBVersion);
                }
            }
        }
        public static void DropDB()
        {
            DbConnection.DropTable<UserMaster>();
            DbConnection.DropTable<CategoryMaster>();
            DbConnection.DropTable<SubCategoryMaster>();
            DbConnection.DropTable<CostInfo>();
            DbConnection.DropTable<DBVersionInfo>();
        }

        private static void MigrationDBData(DBVersionType aDBVersionType)
        {
            if (aDBVersionType == DBVersionType.Version1)
            {
                List<CategoryMaster> categoryMasterList = (from categoryMaster in DBConnHandler.DbConnection.Table<CategoryMaster>() select categoryMaster).ToList();
                List<SubCategoryMaster> subCategoryMasterList = (from subcategoryMaster in DBConnHandler.DbConnection.Table<SubCategoryMaster>() select subcategoryMaster).ToList();
                List<CostInfo> costInfoList = (from costInfo in DBConnHandler.DbConnection.Table<CostInfo>() select costInfo).ToList();

                DbConnection.DropTable<CategoryMaster>();
                DbConnection.DropTable<SubCategoryMaster>();
                DbConnection.DropTable<CostInfo>();
                DbConnection.DropTable<DBVersionInfo>();

                DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS CategoryMaster (CategoryType CHAR(1), Category VARCHAR(50), Description VARCHAR(100), PRIMARY KEY (CategoryType, Category))");
                DbConnection.CreateTable<SubCategoryMaster>();
                DbConnection.CreateTable<CostInfo>();
                DbConnection.CreateTable<DBVersionInfo>();

                categoryMasterList.ForEach(x => x.CategoryType = CategoryType.Expense);
                subCategoryMasterList.ForEach(x => {
                    x.CategoryType = CategoryType.Expense;
                    x.IsRepeat = 0;
                    x.RepeatPeriod = RepeatPeriodType.None;
                    x.RepeatValue = 0;
                    });
                costInfoList.ForEach(x => x.CategoryType = CategoryType.Expense);

                DbConnection.InsertAll(categoryMasterList);
                DbConnection.InsertAll(subCategoryMasterList);
                DbConnection.InsertAll(costInfoList);

                DbConnection.Execute(@"INSERT INTO DBVersionInfo SELECT 2");
            }
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

        private static DBVersionType GetUserDBVersion()
        {
            try
            {
                var dbData = (from Version in DBConnHandler.DbConnection.Table<DBVersionInfo>()
                              select Version).Max();

                if (dbData == null)
                {
                    return DBVersionType.Version1;
                }

                return dbData.Version;
            }
            catch
            {
                return DBVersionType.Version1;
            }
        }
    }
}

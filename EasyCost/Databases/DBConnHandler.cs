using EasyCost.Databases.TableModels;
using EasyCost.Types;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasyCost.Databases
{
    public static class DBConnHandler
    {
        private static DBVersionType LATEST_DB_VERSION = DBVersionType.Version3;
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
                DbConnection.CreateTable<CardMaster>();
                CreateCategoryTable();

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
            DbConnection.DropTable<CardMaster>();
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

                CreateCategoryTable();
                DbConnection.CreateTable<CostInfo>();
                DbConnection.CreateTable<CardMaster>();
                DbConnection.CreateTable<DBVersionInfo>();

                categoryMasterList.ForEach(x => x.CategoryType = CategoryType.Expense);
                subCategoryMasterList.ForEach(x =>
                {
                    x.CategoryType = CategoryType.Expense;
                    x.RepeatYN = "N";
                    x.RepeatPeriod = RepeatPeriodType.None;
                    x.RepeatValue = 0;
                });
                costInfoList.ForEach(x => x.CategoryType = CategoryType.Expense);

                DbConnection.InsertAll(categoryMasterList);
                DbConnection.InsertAll(subCategoryMasterList);
                DbConnection.InsertAll(costInfoList);
                DbConnection.Execute(@"INSERT INTO DBVersionInfo SELECT 3");

                UpdateInitIncomeData();
            }
            else if (aDBVersionType == DBVersionType.Version2)
            {
                List<CostInfo> costInfoList = (from costInfo in DBConnHandler.DbConnection.Table<CostInfo>() select costInfo).ToList();
                costInfoList.ForEach(x => x.CostCard = string.Empty);

                DbConnection.DropTable<CostInfo>();
                DbConnection.DropTable<DBVersionInfo>();

                DbConnection.CreateTable<CostInfo>();
                DbConnection.CreateTable<CardMaster>();
                DbConnection.CreateTable<DBVersionInfo>();

                DbConnection.InsertAll(costInfoList);
                DbConnection.Execute(@"INSERT INTO DBVersionInfo SELECT 3");
            }
        }

        private static void CreateInitialCategory()
        {
            DbConnection.Execute(@"INSERT INTO DBVersionInfo SELECT 3");
            
            UpdateInitExpenseData();
            UpdateInitIncomeData();
        }
        private static void UpdateInitExpenseData()
        {
            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'E', '교통비', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '교통비', '지하철', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '교통비', '버스', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '교통비', '택시', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'E', '식비', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '식비', '아침', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '식비', '점심', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '식비', '저녁', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '식비', '커피', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'E', '공과금', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '공과금', '전기요금', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '공과금', '가스요금', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '공과금', '수도요금', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'E', '취미', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'E', '취미', '책', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'E', '기타', ''");

        }
        private static void UpdateInitIncomeData()
        {
            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'I', '회사', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'I', '회사', '월급', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'I', '회사', '성과급', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'I', '집', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'I', '집', '용돈', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'I', '은행', ''");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'I', '은행', '적금', '', 'N', 0, '', 0");
            DbConnection.Execute(@"INSERT INTO SubCategoryMaster SELECT 'I', '은행', '펀드', '', 'N', 0, '', 0");

            DbConnection.Execute(@"INSERT INTO CategoryMaster SELECT 'I', '기타', ''");
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
        private static void CreateCategoryTable()
        {
            DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS CategoryMaster (CategoryType CHAR(1), Category VARCHAR(50), Description VARCHAR(100), PRIMARY KEY (CategoryType, Category))");
            DbConnection.Execute(@"CREATE TABLE IF NOT EXISTS SubCategoryMaster 
                                        (CategoryType CHAR(1), 
                                         Category VARCHAR(50), 
                                         SubCategory VARCHAR(50),
                                         Description VARCHAR(100), 
                                         RepeatYN CHAR(1),
                                         RepeatPeriod INT,
                                         RepeatDate VARCHAR(5),
                                         RepeatValue INT,                                       
                                        PRIMARY KEY (CategoryType, Category, SubCategory))");
        }
    }
}

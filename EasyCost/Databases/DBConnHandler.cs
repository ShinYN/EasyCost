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
        public static void Initialize()
        {
            DbConnection.CreateTable<UserMaster>();
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
        public static void InitDB()
        {
            DbConnection.DropTable<CategoryMaster>();
            DbConnection.DropTable<SubCategoryMaster>();
            DbConnection.DropTable<CostInfo>();
        }

        public static class Cost
        {
            public static List<CostInfo> GetCostInfo(bool aSelectGroupBy = false)
            {
                if (aSelectGroupBy)
                {
                    return (from c in DbConnection.Table<CostInfo>()
                            where c.UserID == LoginInfo.UserID
                            group c by new { c.CostDate, c.Category, c.SubCategory, c.CostType }
                            into result
                            select new CostInfo
                            {
                                CostDate = result.Key.CostDate,
                                Category = result.Key.Category,
                                SubCategory = result.Key.SubCategory,
                                CostType = result.Key.CostType,
                                Cost = result.Sum(t => t.Cost)
                            }).ToList();
                }
                else
                {
                    return (from c in DbConnection.Table<CostInfo>()
                            where c.UserID == LoginInfo.UserID
                            select c).ToList();
                }
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
            public static List<CategoryMaster> GetCategoryList()
            {
                return (from category in DbConnection.Table<CategoryMaster>()
                        select category).ToList();
            }
            public static void SaveCategory(CategoryMaster aCategoryMaster)
            {
                DbConnection.Insert(aCategoryMaster);
            }
            public static void DeleteCategory(CategoryMaster aCategoryMaster)
            {
                DbConnection.Delete(aCategoryMaster);
            }
            public static void DeleteCategory(string aCategory)
            {
                DbConnection.Execute(string.Format("DELETE FROM CategoryMaster WHERE Category = '{0}'", aCategory));
            }

            public static List<SubCategoryMaster> GetSubCategoryList(string aCategory)
            {
                return (from subCategory in DbConnection.Table<SubCategoryMaster>()
                        where subCategory.Category == aCategory
                        select subCategory).ToList();
            }
            public static void SaveSubCategory(SubCategoryMaster aSubCategoryMaster)
            {
                DbConnection.Insert(aSubCategoryMaster);
            }
            public static void DeleteSubCategory(SubCategoryMaster aSubCategoryMaster)
            {
                DbConnection.Delete(aSubCategoryMaster);
            }
            public static void DeleteSubCategory(string aCategory)
            {
                DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}'", aCategory));
            }
            public static void DeleteSubCategory(string aCategory, string aSubCategory)
            {
                DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}' AND SubCategory = '{1}'", aCategory, aSubCategory));
            }
        }
    }
}

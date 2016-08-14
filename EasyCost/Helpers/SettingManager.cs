using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCost.Helpers
{
    public static class SettingManager
    {
        public static List<CategoryMaster> GetCategoryList()
        {
            return (from category in DBConnHandler.DbConnection.Table<CategoryMaster>()
                    select category).ToList();
        }
        public static void SaveCategory(CategoryMaster aCategoryMaster)
        {
            DBConnHandler.DbConnection.Insert(aCategoryMaster);
        }
        public static void DeleteCategory(CategoryMaster aCategoryMaster)
        {
            DBConnHandler.DbConnection.Delete(aCategoryMaster);
        }
        public static void DeleteCategory(string aCategory)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM CategoryMaster WHERE Category = '{0}'", aCategory));
        }

        public static List<SubCategoryMaster> GetSubCategoryList(string aCategory)
        {
            return (from subCategory in DBConnHandler.DbConnection.Table<SubCategoryMaster>()
                    where subCategory.Category == aCategory
                    select subCategory).ToList();
        }
        public static void SaveSubCategory(SubCategoryMaster aSubCategoryMaster)
        {
            DBConnHandler.DbConnection.Insert(aSubCategoryMaster);
        }
        public static void DeleteSubCategory(SubCategoryMaster aSubCategoryMaster)
        {
            DBConnHandler.DbConnection.Delete(aSubCategoryMaster);
        }
        public static void DeleteSubCategory(string aCategory)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}'", aCategory));
        }
        public static void DeleteSubCategory(string aCategory, string aSubCategory)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}' AND SubCategory = '{1}'", aCategory, aSubCategory));
        }
    }
}

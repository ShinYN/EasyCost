using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace EasyCost.Helpers
{
    public static class SettingManager
    {
        public static List<CategoryMaster> GetCategoryList(string aCategoryType)
        {
            return (from category in DBConnHandler.DbConnection.Table<CategoryMaster>()
                    where category.CategoryType == aCategoryType
                    select category).ToList();
        }
        public static void SaveCategory(CategoryMaster aCategoryMaster)
        {
            DBConnHandler.DbConnection.Insert(aCategoryMaster);
        }
        public static async void DeleteCategory(CategoryMaster aCategoryMaster)
        {
            if (await IsExistsCategory(aCategoryMaster.CategoryType, aCategoryMaster.Category))
            {
                return;
            }
            DBConnHandler.DbConnection.Delete(aCategoryMaster);
            DeleteSubCategory(aCategoryMaster.CategoryType, aCategoryMaster.Category);
        }
        public static async void DeleteCategory(string aCategoryType, string aCategory)
        {
            if (await IsExistsCategory(aCategoryType, aCategory))
            {
                return;
            }
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM CategoryMaster WHERE CategoryType = '{0}' and Category = '{1}'", aCategoryType, aCategory));
        }

        public static List<SubCategoryMaster> GetSubCategoryList(string aCategoryType, string aCategory)
        {
            return (from subCategory in DBConnHandler.DbConnection.Table<SubCategoryMaster>()
                    where subCategory.CategoryType == aCategoryType && subCategory.Category == aCategory
                    select subCategory).ToList();
        }
        public static void SaveSubCategory(SubCategoryMaster aSubCategoryMaster)
        {
            DBConnHandler.DbConnection.Insert(aSubCategoryMaster);
        }
        public static async void DeleteSubCategory(SubCategoryMaster aSubCategoryMaster)
        {
            if (await IsExistsSubCategory(aSubCategoryMaster.CategoryType, aSubCategoryMaster.Category, aSubCategoryMaster.SubCategory))
            {
                return;
            }

            DBConnHandler.DbConnection.Delete(aSubCategoryMaster);
        }
        public static void DeleteSubCategory(string aCategoryType, string aCategory)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE CategoryType = '{0}' AND Category ='{1}'", aCategoryType, aCategory));
        }
        public static async void DeleteSubCategory(string aCategoryType, string aCategory, string aSubCategory)
        {
            if (await IsExistsSubCategory(aCategoryType, aCategory, aSubCategory))
            {
                return;
            }

            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE CategoryType = '{0}' AND Category ='{1}' AND SubCategory = '{2}'"
                , aCategoryType, aCategory, aSubCategory));
        }

        private static async Task<bool> IsExistsSubCategory(string aCategoryType, string aCategory, string aSubCategory)
        {
            if (CostManager.GetCostInfo().Where(x => x.CategoryType == aCategoryType)
                                         .Where(x => x.Category == aCategory)
                                         .Where(x => x.SubCategory == aSubCategory)
                                         .Count() != 0)
            {
                var dialog = new MessageDialog("이미 등록된 지출 정보가 있습니다. 지출 정보를 먼저 삭제해 주세요.");
                dialog.Title = "확인";
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                var res = await dialog.ShowAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        private static async Task<bool> IsExistsCategory(string aCategoryType, string aCategory)
        {
            if (CostManager.GetCostInfo().Where(x => x.CategoryType == aCategoryType)
                                         .Where(x => x.Category == aCategory)
                                         .Count() != 0)
            {
                var dialog = new MessageDialog("이미 등록된 지출 정보가 있습니다. 지출 정보를 먼저 삭제해 주세요.");
                dialog.Title = "확인";
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                var res = await dialog.ShowAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

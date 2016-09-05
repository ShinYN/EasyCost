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
        public static List<CategoryMaster> GetCategoryList()
        {
            return (from category in DBConnHandler.DbConnection.Table<CategoryMaster>()
                    select category).ToList();
        }
        public static void SaveCategory(CategoryMaster aCategoryMaster)
        {
            DBConnHandler.DbConnection.Insert(aCategoryMaster);
        }
        public static async void DeleteCategory(CategoryMaster aCategoryMaster)
        {
            if (await IsExistsCategory(aCategoryMaster.Category))
            {
                return;
            }
            DBConnHandler.DbConnection.Delete(aCategoryMaster);
            DeleteSubCategory(aCategoryMaster.Category);
        }
        public static async void DeleteCategory(string aCategory)
        {
            if (await IsExistsCategory(aCategory))
            {
                return;
            }
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
        public static async void DeleteSubCategory(SubCategoryMaster aSubCategoryMaster)
        {
            if (await IsExistsSubCategory(aSubCategoryMaster.Category, aSubCategoryMaster.SubCategory))
            {
                return;
            }

            DBConnHandler.DbConnection.Delete(aSubCategoryMaster);
        }
        public static void DeleteSubCategory(string aCategory)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}'", aCategory));
        }
        public static async void DeleteSubCategory(string aCategory, string aSubCategory)
        {
            if (await IsExistsSubCategory(aCategory, aSubCategory))
            {
                return;
            }

            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM SubCategoryMaster WHERE Category ='{0}' AND SubCategory = '{1}'", aCategory, aSubCategory));
        }

        private static async Task<bool> IsExistsSubCategory(string aCategory, string aSubCategory)
        {
            if (CostManager.GetCostInfo().Where(x => x.Category == aCategory)
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
        private static async Task<bool> IsExistsCategory(string aCategory)
        {
            if (CostManager.GetCostInfo().Where(x => x.Category == aCategory)
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

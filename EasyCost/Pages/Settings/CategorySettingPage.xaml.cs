using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace EasyCost.Pages.Settings
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class CategorySettingPage : Page
    {
        public CategorySettingPage()
        {
            this.InitializeComponent();

            DisplayCategoryList();
        }

        private void DisplayCategoryList()
        {
            lsvCategory.Items.Clear();
            SettingManager.GetCategoryList().ForEach(elem => lsvCategory.Items.Add(new { Category = elem.Category }));

            if (lsvCategory.Items.Count() == 0)
            {
                lsvSubCategory.Items.Clear();
            }
            else
            {
                lsvCategory.SelectedIndex = 0;
                DisplaySubCategoryList();
            }
        }

        private void DisplaySubCategoryList()
        {
            lsvSubCategory.Items.Clear();
            dynamic category = lsvCategory.SelectedValue;
            string categorystring = category.Category;
            SettingManager.GetSubCategoryList(categorystring).ForEach(elem => lsvSubCategory.Items.Add(new { SubCategory = elem.SubCategory }));
        }

        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (lsvCategory.SelectedItems.Count() != 0)
            {
                dynamic category = lsvCategory.SelectedValue;

                SettingManager.DeleteCategory(category.Category);
                DisplayCategoryList();
                txtCategory.Text = string.Empty;
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtCategory.Text.Trim() == string.Empty)
            {
                return;
            }

            SettingManager.SaveCategory(new Databases.TableModels.CategoryMaster
            {
                Category = txtCategory.Text.Trim(), Description = string.Empty
            });

            DisplayCategoryList();
            txtCategory.Text = string.Empty;
        }

        private void btnRemoveSubCategory_Click(object sender, RoutedEventArgs e)
        {
            if (lsvSubCategory.SelectedItems.Count() != 0)
            {
                dynamic category = lsvCategory.SelectedValue;
                dynamic subCategory = lsvSubCategory.SelectedValue;

                SettingManager.DeleteSubCategory(new SubCategoryMaster { Category = category.Category, SubCategory = subCategory.SubCategory });
                DisplaySubCategoryList();
                txtSubCategory.Text = string.Empty;
            }
        }

        private void btnAddSubCategory_Click(object sender, RoutedEventArgs e)
        {
            if (txtSubCategory.Text.Trim() == string.Empty)
            {
                return;
            }

            dynamic category = lsvCategory.SelectedValue;
            SettingManager.SaveSubCategory(new Databases.TableModels.SubCategoryMaster
            {
                Category = category.Category, SubCategory = txtSubCategory.Text.Trim(), Description = string.Empty
            });

            DisplaySubCategoryList();
            txtSubCategory.Text = string.Empty;
        }

        private void lsvCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvCategory.SelectedItems.Count != 0)
            {
                DisplaySubCategoryList();
            }
        }
    }
}

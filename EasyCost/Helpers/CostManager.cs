using EasyCost.Bases.Login;
using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Syncfusion.XlsIO;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace EasyCost.Helpers
{
    public static class CostManager
    {
        public static List<CostInfo> GetCostInfo(bool aSelectGroupBy = false)
        {
            if (aSelectGroupBy)
            {
                return (from c in DBConnHandler.DbConnection.Table<CostInfo>()
                        where c.UserID == LoginInfo.UserID
                        group c by new { CostDate = c.CostDate.ToString("yyyyMMdd"), c.Category, c.SubCategory, c.CostType }
                        into result
                        orderby result.Key.Category, result.Key.SubCategory, result.Key.CostType
                        select new CostInfo
                        {
                            CostDate = DateTime.ParseExact(result.Key.CostDate, "yyyyMMdd", null),
                            Category = result.Key.Category,
                            SubCategory = result.Key.SubCategory,
                            CostType = result.Key.CostType,
                            Cost = result.Sum(c => c.Cost)
                        }).ToList();
            }
            else
            {
                return (from c in DBConnHandler.DbConnection.Table<CostInfo>()
                        where c.UserID == LoginInfo.UserID
                        orderby c.CostDate descending
                        select c).ToList();
            }
        }
        public static List<CostInfo> GetCostInfo(InquiryType aInquiryType, bool aSelectGroupBy = false)
        {
            List<CostInfo> costInfo = new List<CostInfo>();
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;

            if (aInquiryType == InquiryType.Today)
            {
                costInfo = CostManager.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).ToList();
            }
            else if (aInquiryType == InquiryType.Week)
            {
                costInfo = CostManager.GetCostInfo(aSelectGroupBy).Where(elem =>
                    cal.GetWeekOfYear(elem.CostDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) ==
                    cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();
            }
            else if (aInquiryType == InquiryType.Month)
            {
                costInfo = CostManager.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.ToString("yyyyMM") == DateTime.Now.ToString("yyyyMM")).ToList();
            }
            else if (aInquiryType == InquiryType.Year)
            {
                costInfo = CostManager.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.Year == DateTime.Now.Year).ToList();
            }
            else if (aInquiryType == InquiryType.All)
            {
                costInfo = CostManager.GetCostInfo(aSelectGroupBy);
            }

            return costInfo;
        }
        public static List<CostInfo> GetCostInfo(DateTime aFromDate, DateTime aToDate, bool aSelectGroupBy = false)
        {
            return CostManager.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate >= aFromDate)
                                                          .Where(elem => elem.CostDate <= aToDate)
                                                          .ToList();
        }
        public static List<CostInfo> GetCostInfo(DateTime aSpecificDate, bool aSelectGroupBy = false)
        {
            return CostManager.GetCostInfo(aSelectGroupBy).Where(elem => elem.CostDate.ToString("yyyyMMdd") == aSpecificDate.ToString("yyyyMMdd")).ToList();
        }

        public static void SaveConstInfo(CostInfo aCostInfo)
        {
            DBConnHandler.DbConnection.Insert(aCostInfo);
        }
        public static void UpdateCostInfo(CostInfo aCostInfo)
        {
            DBConnHandler.DbConnection.Update(aCostInfo);
        }
        public static void DeleteCostInfo(CostInfo aCostInfo)
        {
            DBConnHandler.DbConnection.Delete(aCostInfo);
        }
        public static void DeleteCostInfo(int aID)
        {
            DBConnHandler.DbConnection.Execute(string.Format("DELETE FROM CostInfo WHERE Id = {0}", aID));
        }
        public static async Task ExportToExcel(List<CostInfo> aCostInfoList)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                worksheet.Range["A1"].Text = "사용 날짜";
                worksheet.Range["B1"].Text = "지출 분류";
                worksheet.Range["C1"].Text = "세부 분류";
                worksheet.Range["D1"].Text = "타입";
                worksheet.Range["E1"].Text = "지출 내역";
                worksheet.Range["F1"].Text = "지출 금액";
                
                int rowIndex = 2;
                foreach (CostInfo costInfo in aCostInfoList)
                {
                    worksheet.Range["A" + rowIndex].Text = costInfo.CostDate.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Range["B" + rowIndex].Text = costInfo.Category;
                    worksheet.Range["C" + rowIndex].Text = costInfo.SubCategory;
                    worksheet.Range["D" + rowIndex].Text = costInfo.CostType;
                    worksheet.Range["E" + rowIndex].Text = costInfo.Description;
                    worksheet.Range["F" + rowIndex].Text = costInfo.Cost.ToString();

                    rowIndex++;
                }
                
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                savePicker.SuggestedFileName = "공감가계부_지출내역";
                savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
                StorageFile storageFile = await savePicker.PickSaveFileAsync();

                if (storageFile != null)
                {
                    await workbook.SaveAsAsync(storageFile);
                    workbook.Close();

                    MessageDialog msgDialog = new MessageDialog("생성된 파일을 열어보시겠습니까?", "파일이 성공적으로 생성되었습니다.");

                    UICommand yesCmd = new UICommand("예");
                    msgDialog.Commands.Add(yesCmd);
                    UICommand noCmd = new UICommand("아니요");
                    msgDialog.Commands.Add(noCmd);
                    IUICommand cmd = await msgDialog.ShowAsync();
                    if (cmd == yesCmd)
                    {
                        bool success = await Windows.System.Launcher.LaunchFileAsync(storageFile);
                    }
                }
            }
        }
    }
}

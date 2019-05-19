using EasyCost.Bases.Login;
using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using EasyCost.Types;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace EasyCost.Helpers
{
    public static class CostManager
    {
        public static List<CostInfo> GetCostInfo(string aCategoryType = "")
        {
            List<CostInfo> costInfoList;
            costInfoList = (from c in DBConnHandler.DbConnection.Table<CostInfo>()
                            where c.UserID == LoginInfo.UserID
                            orderby c.CostDate descending
                            select c).ToList();

            if (aCategoryType != string.Empty)
            {
                costInfoList = costInfoList.Where(x => x.CategoryType == aCategoryType).ToList();
            }

            return costInfoList;
        }
        public static List<CostInfo> GetCostInfo(InquiryType aInquiryType, string aCategoryType = "")
        {
            List<CostInfo> costInfo = new List<CostInfo>();
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;

            if (aInquiryType == InquiryType.Today)
            {
                costInfo = CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).ToList();
            }
            else if (aInquiryType == InquiryType.Week)
            {
                costInfo = CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate.Year == DateTime.Now.Year)
                                                                                 .Where(elem =>
                                                                                    cal.GetWeekOfYear(elem.CostDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) ==
                                                                                    cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();
            }
            else if (aInquiryType == InquiryType.Month)
            {
                costInfo = CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate.ToString("yyyyMM") == DateTime.Now.ToString("yyyyMM")).ToList();
            }
            else if (aInquiryType == InquiryType.Year)
            {
                costInfo = CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate.Year == DateTime.Now.Year).ToList();
            }
            else if (aInquiryType == InquiryType.All)
            {
                costInfo = CostManager.GetCostInfo(aCategoryType);
            }

            return costInfo;
        }
        public static List<CostInfo> GetCostInfo(DateTime aFromDate, DateTime aToDate, string aCategoryType = "")
        {
            return CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate >= aFromDate)
                                                         .Where(elem => elem.CostDate <= aToDate)
                                                         .ToList();
        }
        public static List<CostInfo> GetCostInfo(DateTime aSpecificDate, string aCategoryType = "")
        {
            return CostManager.GetCostInfo(aCategoryType).Where(elem => elem.CostDate.ToString("yyyyMMdd") == aSpecificDate.ToString("yyyyMMdd")).ToList();
        }

        public static void SaveConstInfo(CostInfo aCostInfo)
        {
            DBConnHandler.DbConnection.Insert(aCostInfo);
        }
        public static void UpdateCostInfo(CostInfo aCostInfo)
        {
            DBConnHandler.DbConnection.Update(aCostInfo);
        }
        public static void UpdateCostInfoCardName(string aOrgCardName, string aNewCardName)
        {
            DBConnHandler.DbConnection.Execute($"UPDATE CostInfo SET CostCard = '{aNewCardName}' WHERE CostCard = '{aOrgCardName}' ");
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
                worksheet.Range["B1"].Text = "수입/지출";
                worksheet.Range["C1"].Text = "분류";
                worksheet.Range["D1"].Text = "세부 분류";
                worksheet.Range["E1"].Text = "타입";
                worksheet.Range["F1"].Text = "카드 정보";
                worksheet.Range["G1"].Text = "내역";
                worksheet.Range["H1"].Text = "금액";
                
                int rowIndex = 2;
                foreach (CostInfo costInfo in aCostInfoList)
                {
                    worksheet.Range["A" + rowIndex].Text = costInfo.CostDate.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Range["B" + rowIndex].Text = (costInfo.CategoryType == CategoryType.Expense) ? "지출" : "수입";
                    worksheet.Range["C" + rowIndex].Text = costInfo.Category;
                    worksheet.Range["D" + rowIndex].Text = costInfo.SubCategory;
                    worksheet.Range["E" + rowIndex].Text = costInfo.CostType;
                    worksheet.Range["F" + rowIndex].Text = costInfo.CostCard;
                    worksheet.Range["G" + rowIndex].Text = costInfo.Description;
                    worksheet.Range["H" + rowIndex].Text = costInfo.Cost.ToString();

                    rowIndex++;
                }

                worksheet.Range[1, 1, aCostInfoList.Count, 1].ColumnWidth = 20;
                worksheet.Range[1, 2, aCostInfoList.Count, 2].ColumnWidth = 15;
                worksheet.Range[1, 3, aCostInfoList.Count, 3].ColumnWidth = 15;
                worksheet.Range[1, 4, aCostInfoList.Count, 4].ColumnWidth = 15;
                worksheet.Range[1, 5, aCostInfoList.Count, 5].ColumnWidth = 10;
                worksheet.Range[1, 6, aCostInfoList.Count, 6].ColumnWidth = 20;
                worksheet.Range[1, 7, aCostInfoList.Count, 7].ColumnWidth = 20;
                worksheet.Range[1, 8, aCostInfoList.Count, 8].ColumnWidth = 15;

                worksheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["B1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["C1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["D1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["E1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["F1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["G1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["H1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                savePicker.SuggestedFileName = "공감가계부_내역";
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

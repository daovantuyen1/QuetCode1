using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace RMSExternalApi.Commons
{
    public static class ExcelHelper
    {

        public static bool IsDateCell(this ICell cell)
        {
            try
            {
                var dateTime = cell.DateCellValue;
                var dateTimeStr = dateTime.ToString("yyyy_MM_dd").Replace("_", "/");
                if (dateTimeStr == "1900/04/09")// default datetime date.
                    return false;
                DateTime tempInputDate;
                bool rs=  DateTime.TryParseExact(dateTimeStr, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out  tempInputDate);
                return rs;
                

            }
            catch (Exception ex)
            {
            
            }
            return false;

        }

        public static bool IsCellNotNull(this ICell cell)
        {
            try
            {
                if (cell == null) return false;
                if (string.IsNullOrWhiteSpace(cell.ToString())) return false;
                return true;

            }
            catch
            {
                return false;

            }

        }
        // Attemps to read workbook as XLSX, then XLS, then fails.
        public static IWorkbook ReadWorkbook(string path)
        {
            IWorkbook book = null;

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Try to read workbook as XLSX:
                try
                {
                    book = new XSSFWorkbook(fs);
                }
                catch
                {
                    book = null;
                }

                // If reading fails, try to read workbook as XLS:
                if (book == null)
                {
                    book = new HSSFWorkbook(fs);
                }

            }
            catch
            {

            }


            return book;
        }

        public static int GetTotalRowCount(ISheet sheet, bool warrant = false)
        {
            IRow headerRow = sheet.GetRow(0);
            if (headerRow != null)
            {
                int rowCount = sheet.LastRowNum + 1;
                return rowCount;
            }
            return 0;
        }

        public static bool WriteAndSaveWorkBook(IWorkbook workbook, string path, ref string err)
        {
            try
            {
                err = String.Empty;
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message + Environment.NewLine + ex.StackTrace;
                return false;
            }

        }

        public static void CreateNewExcelFile()
        {
            #region for .xlsx file 
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("My sheet");
            IRow row = sheet.CreateRow(0);
            ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("TEN");
            ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("TUOI");
            IRow row1 = sheet.CreateRow(1);
            ICell cell10 = row1.CreateCell(0);
            cell10.SetCellValue("TUYEN");
            ICell cell11 = row1.CreateCell(1);
            cell11.SetCellValue("12");

            using (FileStream stream = new FileStream("outfile.xlsx", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }

            #endregion

            #region for .xls file 
            IWorkbook workbook1 = new HSSFWorkbook();
            ISheet sheet1 = workbook1.CreateSheet("My sheet");
            IRow row1_0 = sheet1.CreateRow(0);
            ICell cell1_0 = row1_0.CreateCell(0);
            cell1_0.SetCellValue("TEN");
            using (FileStream stream = new FileStream("outfile1.xls", FileMode.Create, FileAccess.Write))
            {
                workbook1.Write(stream);
            }

            #endregion 
        }

        public static void runFormual()
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("My sheet");
            IRow TitleRows = sheet.CreateRow(0);
            TitleRows.CreateCell(0).SetCellValue("TEN");
            TitleRows.CreateCell(1).SetCellValue("DIEM");

            IRow tuyenRow = sheet.CreateRow(1);
            tuyenRow.CreateCell(0).SetCellValue("TUYEN");
            tuyenRow.CreateCell(1).SetCellValue(12);

            IRow toanRow = sheet.CreateRow(2);
            toanRow.CreateCell(0).SetCellValue("TOAN");
            toanRow.CreateCell(1).SetCellValue(3);

            IRow hungRow = sheet.CreateRow(3);
            hungRow.CreateCell(0).SetCellValue("HUNG");
            hungRow.CreateCell(1).SetCellValue(5);

            IRow diemTBRow = sheet.CreateRow(4);
            diemTBRow.CreateCell(0).SetCellValue("DIEM TB");
            diemTBRow.CreateCell(1).SetCellFormula("AVERAGE(B2:B4)"); // run formula in this cell.
            string err = "";
            WriteAndSaveWorkBook(workbook, "formual.xlsx", ref err);
        }

        public static void setCellStyle()
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("My sheet");
            IRow TitleRows = sheet.CreateRow(0);
            ICell cell = TitleRows.CreateCell(0);
            cell.SetCellValue("tuyến");

            IFont font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 11;
            font.Color = IndexedColors.Red.Index;
            font.Charset = 0;


            /**
           // * ANSI character Set
           // */
            //public static byte ANSI_CHARSET = 0;

            //    /**
            //     * Default character Set.
            //     */
            //    public static byte DEFAULT_CHARSET = 1;

            //    /**
            //     * Symbol character Set
            //     */
            //    public static byte SYMBOL_CHARSET = 2;


            ICellStyle boldStyle = workbook.CreateCellStyle();
            boldStyle.SetFont(font);

            cell.CellStyle = boldStyle;

            string err = "";
            WriteAndSaveWorkBook(workbook, "cellStyle.xlsx", ref err);
        }




        public static void ExampleReadExcel()
        {
            string fullPath = "aaa.xlsx";
            var book = ExcelHelper.ReadWorkbook(fullPath);
            var sheet = book.GetSheetAt(0);
            int totalRow = ExcelHelper.GetTotalRowCount(sheet);
            List<string> lsData = new List<string>();
            for (int i = 0; i < totalRow; i++)
            {
                if (i > 1)
                {
                    var row = sheet.GetRow(i);
                    lsData.Add(row.GetCell(0) == null ? "" : row.GetCell(0).ToString().Trim().ToUpper());
                }
            }

        }
       public static  void CreateCell(IRow currentRow, int CellIndex, string Value)
        {
            ICell Cell = currentRow.CreateCell(CellIndex);
            Cell.SetCellType(CellType.String);
            Cell.SetCellValue(Value);
        }
        public static HttpResponseMessage ExampleWriteExcel()
        {
           

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Dữ liệu CNV");
            // set header 
            IRow row0 = sheet.CreateRow(0);
            CreateCell(row0, 0, "Pháp nhân");
            //
            List<string> lsDat = new List<string>();
            lsDat.Add("funning");
            lsDat.Add("fuyu");

            if (lsDat != null && lsDat.Count > 0)
            {
                for (int i = 0; i < lsDat.Count; i++)
                {
                    var emp = lsDat[i];
                    IRow row1 = sheet.CreateRow(i + 1);

                    CreateCell(row1, 0, emp);
                }
            }

            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = "aaa.xls";  // must be xls
                byte[] bytes = exportData.ToArray();

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(bytes)
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = saveAsFileName
                    };
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/vnd.ms-excel");
                return result;
            }


        }
    }
}
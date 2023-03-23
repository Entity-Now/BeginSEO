using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BeginSEO.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BeginSEO.Utils
{
    public class ExcelUtils
    {
        public static void OpenExcel<T>()
        {

        }
        public static void ImportToList(string fileName)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(fileName).ToLower();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    workbook = null;
                    return;
                }
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    
                    // 获取行数
                    var RowCount = sheet.LastRowNum;
                    for (int j = 0; j < RowCount; j++)
                    {
                        // cell
                        var row = sheet.GetRow(j);
                        foreach (var item in row.Cells)
                        {
                            MessageBox.Show(item.StringCellValue);
                        }
                    }
                }

                return;
            }
        }

        //获取单元格类型
        static object GetValueType(ICell cell)
        {
            if (cell == null) return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
    }
}

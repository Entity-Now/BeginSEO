using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BeginSEO.Attributes;
using BeginSEO.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Cms;

namespace BeginSEO.Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">反射对象，将Excell的数据映射到此对象</typeparam>
    public class ExcelUtils<T> where T : class
    {
        IWorkbook workbook { get; set; }
        string FileName { get; set; }
        Dictionary<int, string> HeadNames = new Dictionary<int, string>();
        public ExcelUtils(string _FileName) 
        {
            FileName = _FileName;
            OpenFile();
        }
        public void OpenFile()
        {
            try
            {
                string fileExt = Path.GetExtension(FileName).ToLower();
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
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
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("警告", e.Message);
            }
        }
        /// <summary>
        /// 获取Excel的表头（sheet的第一行为表头）
        /// </summary>
        public void GetHeads()
        {
            // 获取泛型的属性
            var ref_Head_Name = typeof(T).GetProperties()
                                         .Where(I => I.GetCustomAttribute<ExcellDataAttribute>() != null)
                                         .Select(I=> I.GetCustomAttribute<ExcellDataAttribute>().Headers);
            // 获取
            var sheet = workbook.GetSheetAt(0);
            var row = sheet.GetRow(0);
            for (int j = 0; j < row.LastCellNum; j++)
            {
                var cell = row.GetCell(j);
                var FindHead = ref_Head_Name.FirstOrDefault(I => I.Contains(GetValueType(cell).ToString()));
                if (FindHead != null)
                {
                    HeadNames.Add(j, FindHead[0]);
                }
            }
        }
        public void ImportToList(string fileName)
        {
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);

                // 获取行数
                var RowCount = sheet.LastRowNum;
                for (int j = 0; j < RowCount; j++)
                {
                    // cell
                    var row = sheet.GetRow(j);
                    if (row == null)
                    {
                        continue;
                    }
                    foreach (var item in row.Cells)
                    {
                        MessageBox.Show(GetValueType(item).ToString());
                    }
                }
            }
        }

        //获取单元格类型
        object GetValueType(ICell cell)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LinqToExcel;
using 替换关键词.Data;

namespace 替换关键词.Utils
{
    public class ExcelUtils
    {
        public static void OpenExcel<T>(string Path, List<T> GetValue, T SpareTire)
        {
            string a = "";
            var ExcelData = new ExcelQueryFactory(Path);
            // 循环每个工作表
            ExcelData.GetWorksheetNames().ToList().ForEach(name =>
            {
                // 获取每个表
                var Find = ExcelData.Worksheet<T>(name);
                foreach (var item in Find)
                {
                    GetValue.Add(item);     
                }
                // 添加分隔符
                if (SpareTire != null)
                {
                    GetValue.Add(SpareTire);
                }
            });
            //MessageBox.Show(SheetList[0].ColumnNames.ElementAt(0));
        }
    }
}

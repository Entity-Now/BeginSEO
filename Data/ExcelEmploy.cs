using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel.Attributes;
using LinqToExcel;

namespace 替换关键词.Data
{
    public class ExcelEmploy
    {
        [ExcelColumn("标题")]
        public string Name { get; set; }
        [ExcelColumn("发布时间")]
        public DateTime PublishTime { get; set; }
        [ExcelColumn("链接")]
        public string Link { get; set; }
        [ExcelColumn("排名")]
        public int Order { get; set; }
        [ExcelColumn("收录")]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Split { get; set; }
    }
}

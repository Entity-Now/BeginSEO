using BeginSEO.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Data
{
    public class ExcelEmploy
    {
        [ExcellData("标题")]
        public string Name { get; set; }
        [ExcellData("发布时间")]
        public DateTime PublishTime { get; set; }
        [ExcellData("链接","URL", "PC URL","地址", "网址")]
        public string Link { get; set; }
        [ExcellData("排名")]
        public int Order { get; set; }
        [ExcellData("收录")]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Split { get; set; }
    }
}

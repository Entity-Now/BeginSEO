using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118
{
    /// <summary>
    /// 只能原创结果
    /// </summary>
    public class OriginalResult
    {
        /// <summary>
        /// 相似度
        /// </summary>
        public double contrastValue { get; set; }
        public string NewValue { get; set; }
        public bool OriginalStatus { get; set; }
        public bool AkeyStatus { get; set; }
        public bool NewOriginalStatus { get; set; }
        public string OriginalError { get; set; }
        public string AkeyError { get; set; }
        public string NewOriginalError { get; set; }
    }
}

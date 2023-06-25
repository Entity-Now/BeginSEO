using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118
{
    public class Detection
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public Datum[][] data { get; set; }
    }

    public class Datum
    {
        public string Content { get; set; }
        public string OriginalValue { get; set; }
        public string Platform { get; set; }
        public int Sort { get; set; }
        public int ParagraphPosition { get; set; }
    }
}

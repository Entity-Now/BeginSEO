using BeginSEO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public class FuHeGrab : AGrab
    {
        
        public FuHeGrab(string keyWord, int Count = 50) : base(keyWord, Count) 
        {
            
        }
        public override string URL { get; set; } = "jianfei.fh21.com.cn";

        public override Article DeSerializer(string content)
        {
            throw new NotImplementedException();
        }
    }
}

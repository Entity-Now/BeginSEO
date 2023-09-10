using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.Utils._5118.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118
{
    internal class AKeyHandle : I5118Handle
    {
        public _5118Config config { get; set; }
        public AKeyHandle(string _key, string value, string filter)
        {
            var Parameters = new Dictionary<string, string>()
            {
                {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(value))},
                {"sim", "1" },
                {"corewordfilter","1" },
                {"filter",filter}
            };
            config = new _5118Config
            {
                url = "http://apis.5118.com/wyc/akey",
                key = _key,
                parameters = Parameters,
                type = SettingsEnum.ReplaceKeyWord
            };
        }
        public async  Task<_5118Result> HandleText()
        {
            var result = await config.Reqeust<Akey>();
            if (result.errcode != "0")
            {
                return new _5118Result
                {
                    code = _5118Code.Error,
                    msg = result.errmsg
                };
            }
            return new _5118Result
            {
                code = _5118Code.Success,
                msg = result.data,
                like = float.Parse(result.like) * 100
            };
        }
    }
}

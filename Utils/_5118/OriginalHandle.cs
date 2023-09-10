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
    internal class OriginalHandle : I5118Handle
    {
        public _5118Config config { get; set; }
        public OriginalHandle(string key, string innerText, string strict)
        {
            config = new _5118Config
            {
                key = key,
                type = BeginSEO.Data.DataEnum.SettingsEnum.Original,
                url = "http://apis.5118.com/wyc/rewrite",
                parameters = new Dictionary<string, string>
                {
                    {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(innerText))},
                    {"sim", "1" },
                    {"strict",strict}
                }

            };
        }
        public async Task<_5118Result> HandleText()
        {
            var result = await config.Reqeust<Original>();
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

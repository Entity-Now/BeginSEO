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
    internal class DetectionHandle : I5118Handle
    {
        public _5118Config config { get; set; }
        public DetectionHandle(string key, string innerText)
        {
            config = new _5118Config
            {
                key = key,
                url = "https://apis.5118.com/wyc/original",
                type = BeginSEO.Data.DataEnum.SettingsEnum.Detection,
                parameters = new Dictionary<string, string>()
                {
                    {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(innerText))}
                }
            };
        }
        public async Task<_5118Result> HandleText()
        {
            var result = await config.Reqeust<Detection>();
            if(result.errcode != "0")
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
                msg = Tools.SerializerJson(result.data)
            };
        }
    }
}

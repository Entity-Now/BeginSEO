using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using Newtonsoft.Json;

namespace BeginSEO.Utils
{

    public class Original
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string like { get; set; }
        public string data { get; set; }
    }

    public class ReplaceKeyWord
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string corewords { get; set; }
        public string like { get; set; }
        public string data { get; set; }
    }

    public class Contrast
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string like { get; set; }
    }

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


    public static class _5188Tools
    {
        public static async Task<T> Request<T>(SettingsEnum Sm, string url, Dictionary<string, string> body)
        {
            try
            {
                var Context = DataAccess.GetDbContext();
                var Data = Context.Set<Settings>().FirstOrDefault(I => I.Name == Sm.ToString());
                if (Data == null)
                {
                    throw new Exception("您还未添加API的key，请到设置页面添加KEY");
                }
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Data.Value);
                var result = await http.Post(url, body, RequestType.FormData);
                return Tools.DeSerializeJson<T>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 检测文本原创度
        /// </summary>
        /// <returns></returns>
        public static async Task<Detection> Detection(string value)
        {
            var result = await Request<Detection>(SettingsEnum.Detection, "https://apis.5118.com/wyc/original",new Dictionary<string, string>()
            {
                {"txt", WebUtility.UrlEncode(value)}
            });
            return result;
        }
        public static async Task<Original> Original(string value, string strict)
        {
            var result = await Request<Original>(SettingsEnum.Original, "http://apis.5118.com/wyc/rewrite", new Dictionary<string, string>()
            {
                {"txt", WebUtility.UrlEncode(value)},
                {"sim", "1" },
                {"strict",strict}
            });
            return result;
        }
        /// <summary>
        /// 替换关键词
        /// </summary>
        /// <param name="value"></param>
        /// <param name="filter">设置锁词可在一键智能原创时锁住这些词不被替换(用‘|’隔开)</param>
        /// <returns></returns>
        public static async Task<ReplaceKeyWord> ReplaceKeyWord(string value, string filter)
        {
            var result = await Request<ReplaceKeyWord>(SettingsEnum.ReplaceKeyWord, "http://apis.5118.com/wyc/akey", new Dictionary<string, string>()
            {
                {"txt", WebUtility.UrlEncode(value)},
                {"sim", "1" },
                {"corewordfilter","1" },
                {"filter",filter}
            });
            return result;
        }
    }
}

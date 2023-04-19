using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeginSEO.Utils
{

    public class Detection
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string like { get; set; }
        public string data { get; set; }
    }

    public class RepalceKeyWord
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

    public class Original
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
        public static async Task<T> ReplaceKeyword<T>(string token, string url, Dictionary<string, string> body)
        {
            try
            {
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                var result = await http.Post(url, body, RequestType.String);
                return Tools.DeSerializeJson<T>(result);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
        public static string Detection()
        {

            return "";
        }
    }
}

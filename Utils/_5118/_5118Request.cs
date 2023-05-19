using BeginSEO.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using BeginSEO.Utils.Exceptions;

namespace BeginSEO.Utils._5118
{
    public class _5118Request
    {
        public string Key { get; set; }
        public string Url { get; set; }

        public _5118Request(string _key, string url)
        {
            Key = _key;
            Url = url;
        }
        public async Task<T> Request<T>(Dictionary<string, string> Body)
        {
            try
            {
                if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(Url))
                {
                    throw new LoggingException("Key Or Url Is Null");
                }
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Key);
                /// 5秒超时
                //http.Timeout = new TimeSpan(5000);
                var result = await http.Post(Url, Body, RequestType.FormData);
                return Tools.DeSerializeJson<T>(result);
            }
            catch (Exception e)
            {
                throw new LoggingException($"_5188Tools Request error_msg: {e.Message}");
            }
        }
    }
}

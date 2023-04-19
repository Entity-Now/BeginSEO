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
    public static class _5188Tools
    {
        public static async  Task<T> ReplaceKeyword<T>(string token, string url, Dictionary<string,string> body)
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

    }
}

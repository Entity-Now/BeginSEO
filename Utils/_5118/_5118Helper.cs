using BeginSEO.Data.DataEnum;
using BeginSEO.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BeginSEO.SQL;
using BeginSEO.Services;

namespace BeginSEO.Utils._5118
{
    public static class _5118Helper
    {
        public static async Task<T> Reqeust<T>(this _5118Config config)
        {
            string url = config.GetUrl();
            string key = config.GetKey();
            var parameters = config.GetParameters();
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(url))
                {
                    throw new LoggingException("Key Or Url Is Null");
                }
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
                var response = await http.PostAsync(url, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Tools.DeSerializeJson<T>(content);
                }
                throw new LoggingException($"_5118Base 请求失败，状态码为：“{response.StatusCode}”");
            }
            catch (Exception e)
            {
                throw new LoggingException($"_5118Base 程序出错: {e.Message}");
            }
        }
    }
}

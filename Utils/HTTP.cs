using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace 替换关键词.Utils {
    public enum RequestType {
        FormData,
        String,
        Multipart
    }
    public static class HTTP {
        public static Dictionary<string, string> Cookie(this CookieContainer cookieJar, string Url)
        {
            Dictionary<string,string> Cookie = new Dictionary<string, string>();
            // 获取cookie
            var uri = new Uri(Url);
            var responseCookies = cookieJar.GetCookies(uri);
            foreach (Cookie item in responseCookies)
            {
                Cookie.Add(item.Name, item.Value);
            }
            return Cookie;
        }
        public static void Replace(this CookieContainer cookieJar, string Url, ref Dictionary<string, string> Cookies)
        {
            var uri = new Uri(Url);
            foreach (Cookie item in cookieJar.GetCookies(uri))
            {
                int Find = 0;
                foreach (var OldItem in Cookies)
                {
                    if (item.Name == OldItem.Key)
                    {
                        Find = 1;
                        Cookies[OldItem.Value] = item.Value;
                        break;
                    }
                }
            }
        }
        public static async Task<HttpResponseMessage> GetBaiDu(string url, CookieContainer cookieJar = null)
        {
            Dictionary<string, string> Header = new Dictionary<string, string>();
            Header.Add("Host", "www.baidu.com");
            Header.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.41");
            Header.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            Header.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
            Header.Add("Connection", "keep-alive");
            Header.Add("Upgrade-Insecure-Requests", "1");
            Header.Add("Sec-Fetch-Dest", "document");
            Header.Add("Sec-Fetch-Mode", "navigate");
            Header.Add("Sec-Fetch-Site", "none");
            Header.Add("Sec-Fetch-User", "?1");
            Header.Add("sec-ch-ua", @"""Microsoft Edge"";v=""111"", ""Not(A:Brand"";v=""8"", ""Chromium"";v=""111""");
            Header.Add("sec-ch-ua-mobile", "?0");
            Header.Add("sec-ch-ua-platform", @"""Windows""");

            var temp_url = WebUtility.UrlEncode(url);
            string requestUrl = $"https://www.baidu.com/s?wd={temp_url}&rsv_spt=1";
            //string requestUrl = $"https://www.baidu.com";
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = false;
            if (cookieJar != null)
            {
                handler.UseCookies = true;
                handler.CookieContainer = cookieJar;
            }
            handler.AllowAutoRedirect = false;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            foreach (var item in Header)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            return await client.GetAsync(requestUrl);
        }
        public static async Task<string> Post(string url,Dictionary<string,string> Data,RequestType ContentType) {
            HttpClient client = new HttpClient();
            HttpContent content = null;
            if (ContentType == RequestType.FormData) {
                content = new FormUrlEncodedContent(Data);
            }
            else if(ContentType == RequestType.String) {
                string Body = "";
                foreach (var item in Data) {
                    if (string.IsNullOrEmpty(Body)) {
                        Body += $"?{item.Key}={item.Value}";
                    }
                    else {
                        Body += $"&{item.Key}={item.Value}";
                    }
                }
                content = new StringContent(Body);
            }

            var result = await client.PostAsync(url, content);
            if (!result.IsSuccessStatusCode) {
                return string.Empty;
            }
            return await result.Content.ReadAsStringAsync();
        }
    }
}

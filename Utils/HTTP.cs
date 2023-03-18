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
        public static List<string> User_Agent = new List<string>()
        {
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.41",
           //"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0",
           //"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36"
        };
        public static void Replace(this CookieContainer CookieJar, string Url, ref List<Cookie> Cookies)
        {
            var uri = new Uri(Url);
            var responseCookie = CookieJar.GetCookies(uri);
            foreach (Cookie item in responseCookie)
            {
                var findCookie = Cookies.Find(I => I.Name == item.Name);
                if (findCookie != null && findCookie != default)
                {
                    findCookie = item;
                }
                else
                {
                    Cookies.Add(item);
                }
            }
        }
        public static async Task<HttpResponseMessage> GetBaiDu(string url, CookieContainer cookieJar, List<Cookie> Cookies)
        {
            Dictionary<string, string> Header = new Dictionary<string, string>();
            Header.Add("Host", "www.baidu.com");
            //Header.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0");
            Header.Add("User-Agent", User_Agent[new Random().Next(0, User_Agent.Count)]);
            Header.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            Header.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
            Header.Add("Connection", "keep-alive");
            Header.Add("Upgrade-Insecure-Requests", "1");
            Header.Add("Sec-Fetch-Dest", "document");
            Header.Add("Sec-Fetch-Mode", "navigate");
            Header.Add("Sec-Fetch-Site", "none");
            Header.Add("Sec-Fetch-User", "?1");
            //Header.Add("sec-ch-ua", @"""Microsoft Edge"";v=""111"", ""Not(A:Brand"";v=""8"", ""Chromium"";v=""111""");
            //Header.Add("sec-ch-ua-mobile", "?0");
            //Header.Add("sec-ch-ua-platform", @"""Windows""");

            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = true;
            handler.CookieContainer = cookieJar;
            if (Cookies.Count > 0)
            {
                foreach (var item in Cookies)
                {
                    cookieJar.Add(item);
                }
            }
            handler.AllowAutoRedirect = false;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            foreach (var item in Header)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            return await client.GetAsync(url);
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

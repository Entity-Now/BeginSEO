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
using System.IO;
using BrotliSharpLib;
using System.IO.Compression;
using System.Windows;
using BeginSEO.Model;

namespace BeginSEO.Utils {
    public enum RequestType {
        FormData,
        String,
        Multipart
    }
    public static class HTTP {
        public static List<string> User_Agent = new List<string>()
        {
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.41",
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0",
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.25 Safari/537.36 Core/1.70.3732.400 QQBrowser/10.5.3819.400",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.133 Safari/534.16",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:34.0) Gecko/20100101 Firefox/34.0",
            "Mozilla/5.0 (X11; U; Linux x86_64; zh-CN; rv:1.9.2.10) Gecko/20100922 Ubuntu/10.10 (maverick) Firefox/3.6.10",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36 OPR/26.0.1656.60",
            "Mozilla/5.0 (Windows NT 5.1; U; en; rv:1.8.1) Gecko/20061208 Firefox/2.0.0 Opera 9.50",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; en) Opera 9.50",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.57.2 (KHTML, like Gecko) Version/5.1.7 Safari/534.57.2",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; QQBrowser/7.0.3698.400)",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 732; .NET4.0C; .NET4.0E)",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.84 Safari/535.11 SE 2.X MetaSr 1.0",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SV1; QQDownload 732; .NET4.0C; .NET4.0E; SE 2.X MetaSr 1.0)",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.122 UBrowser/4.0.3214.0 Safari/537.36"
        };
        public static List<string> Baidu_Url = new List<string>()
        {
            "www.baidu.com"
            , "180.97.33.107", "180.97.33.108","180.101.50.188","14.215.177.38","14.215.177.39","104.193.88.77","104.193.88.123"
        };
        /// <summary>
        /// 从响应结果中获取cookies
        /// </summary>
        /// <param name="CookieJar"></param>
        /// <param name="Url"></param>
        /// <param name="Cookies"></param>
        public static void Replace(this CookieContainer CookieJar, string Url, ref List<Cookie> Cookies)
        {
            var uri = new Uri(Url);
            var responseCookie = CookieJar.GetCookies(uri);
            foreach (Cookie item in responseCookie)
            {
                if (item.Name == "BAIDUID_BFESS")
                {
                    File.WriteAllText("BAIDUID_BFESS.txt",item.Value);
                }
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
            if (Cookies.Find(I=>I.Name == "BAIDUID_BFESS") == null)
            {
                if (File.Exists("BAIDUID_BFESS.txt"))
                {
                    var BAIDUID = File.ReadAllText("BAIDUID_BFESS.txt");
                    Cookies.Add(new Cookie("BAIDUID_BFESS", BAIDUID) { Domain = "baidu.com"});
                }
            }
        }
        public static void GetCookies(this HttpResponseMessage result,string Host, ref Dictionary<string, string> Cookies)
        {
            var head = result.Headers.GetValues("Set-Cookie");
            string GetCookieValue = "";
            foreach (var item in head)
            {
                GetCookieValue += Regex.Match(item, @".*?;").Value;
            }
            Cookies[Host] = GetCookieValue;
        }

        public static async Task<HttpResponseMessage> GetBaiDu(string url, string Cookies)
        {
            string Host = Regex.Match(url, @"(?<=https?:\/\/)\S+?(?=\/)").Value;
            Dictionary<string, string> Header = new Dictionary<string, string>();
            Header.Add("Host", Host);
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
            Header.Add("Cookie", Cookies);
            //Header.Add("sec-ch-ua", @"""Microsoft Edge"";v=""111"", ""Not(A:Brand"";v=""8"", ""Chromium"";v=""111""");
            //Header.Add("sec-ch-ua-mobile", "?0");
            //Header.Add("sec-ch-ua-platform", @"""Windows""");

            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;
            // 不使用CookiesContainer
            handler.UseCookies = false;

            // 禁止重定向
            handler.AllowAutoRedirect = false;
                var client = new HttpClient(handler);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            foreach (var item in Header)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            var response = await client.GetAsync(url);
            // 判断响应头编码是否是br
            if (response.Content.Headers.ContentEncoding.Contains("br"))
            {
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                // 解码
                response.Content = new ByteArrayContent(Brotli.DecompressBuffer(buffer, 0, buffer.Length));
            }

            return response;
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

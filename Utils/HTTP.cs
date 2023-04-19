using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.IO;
using BrotliSharpLib;
using BeginSEO.Model;
using BeginSEO.SQL;
using BeginSEO.Data;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.Remoting.Contexts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;

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
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.122 UBrowser/4.0.3214.0 Safari/537.36",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.2 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.50 Safari/525.19",
            "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1090.0 Safari/536.6",
            "Mozilla/5.0 (Windows; U; Windows NT 5.2; eu) AppleWebKit/530.4 (KHTML, like Gecko) Chrome/2.0.172.0 Safari/530.4",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.153.0 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.8 (KHTML, like Gecko) Chrome/7.0.521.0 Safari/534.8",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.3 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.8 (KHTML, like Gecko) Chrome/6.0.397.0 Safari/533.8",
            "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.27 Safari/525.13",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/530.5 (KHTML, like Gecko) Chrome/2.0.172.43 Safari/530.5",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.3 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.6 Safari/525.13",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/530.5 (KHTML, like Gecko) Chrome/2.0.172.39 Safari/530.5",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/4.0.211.7 Safari/532.0",
            "Mozilla/6.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.27 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/530.5 (KHTML, like Gecko) Chrome/2.0.172.2 Safari/530.5",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.601.0 Safari/534.14",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.0 Safari/532.5",
            "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.17 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
            "Mozilla/6.0 (Windows; U; Windows NT 6.0; en-US) Gecko/2009032609 (KHTML, like Gecko) Chrome/2.0.172.6 Safari/530.7",
            "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100 Safari/534.30",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.1 Safari/532.0",
            "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/18.6.872.0 Safari/535.2 UNTRUSTED/1.0 3gpp-gba UNTRUSTED/1.0",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/533.3 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/533.3",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.21 Safari/532.0",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.198.0 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/530.1 (KHTML, like Gecko) Chrome/2.0.169.0 Safari/530.1",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.8 (KHTML, like Gecko) Chrome/6.0.397.0 Safari/533.8",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.59 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; it-IT) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.25 Safari/532.5",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.2 (KHTML, like Gecko) Chrome/4.0.222.4 Safari/532.2",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.30 Safari/525.13",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.21 Safari/532.0",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.700.3 Safari/534.24",
            "Mozilla/5.0 (Windows NT 7.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.3 (KHTML, like Gecko) Chrome/4.0.227.0 Safari/532.3",
            "Mozilla/5.0 (Windows NT 4.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.6 Safari/537.11",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.0 Safari/534.13",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.152.0 Safari/525.19",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.860.0 Safari/535.2",
            "Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/533.16 (KHTML, like Gecko) Chrome/5.0.335.0 Safari/533.16",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.66 Safari/535.11",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; de-DE) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/7.0.540.0 Safari/534.10",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.601.0 Safari/534.14",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.151.0 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.198.0 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/0.2.153.0 Safari/525.19",
            "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.93 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.17 Safari/537.36",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.4 Safari/532.1",
            "Mozilla/5.0 (Windows NT 6.0; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.45 Safari/535.19",
            "Mozilla/5.0 (Windows NT 6.1; en-US) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.750.0 Safari/534.30",
            "Mozilla/6.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.27 Safari/532.0",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.461.0 Safari/534.3",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.669.0 Safari/534.20",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/532.0 (KHTML, like Gecko) Chrome/3.0.195.21 Safari/532.0",
            "Mozilla/5.0 (Windows NT 6.4; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2225.0 Safari/537.36",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1) AppleWebKit/526.3 (KHTML, like Gecko) Chrome/14.0.564.21 Safari/526.3",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.600.0 Safari/534.14",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.3 (KHTML, like Gecko) Chrome/6.0.459.0 Safari/534.3",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/530.7 (KHTML, like Gecko) Chrome/2.0.176.0 Safari/530.7",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.864.0 Safari/535.2",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.21 (KHTML, like Gecko) Chrome/11.0.682.0 Safari/534.21",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.792.0 Safari/535.1",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.307.1 Safari/532.9",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.3 (KHTML, like Gecko) Chrome/4.0.227.0 Safari/532.3",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.107 Safari/535.1",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.53 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/525.19 (KHTML, like Gecko) Chrome/1.0.154.46 Safari/525.19",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/530.5 (KHTML, like Gecko) Chrome/2.0.172.43 Safari/530.5",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.872.0 Safari/535.2",
            "Mozilla/5.0 (Windows NT 5.2) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.794.0 Safari/535.1",
            "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/530.5 (KHTML, like Gecko) Chrome/2.0.172.2 Safari/530.5",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.2117.157 Safari/537.36"
        };
        public static List<string> Baidu_Url = new List<string>()
        {
            "www.baidu.com",
            "180.97.33.107",
            "180.97.33.108",
            "180.101.50.188",
            "180.101.50.242",
            "39.156.69.79",
            "39.156.69.80",
            //"220.181.38.149",
            //"220.181.38.148",
            "123.125.115.111",
            "123.125.115.110",
            "104.193.88.77",
            "104.193.88.123",
            "14.215.177.38",
            "14.215.177.39"
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
        public static void GetCookies(this HttpResponseMessage result,string Host)
        {
            var head = result.Headers.GetValues("Set-Cookie");
            foreach (var item in head)
            {
                string GetCookieValue = Regex.Match(item, @".*?;").Value;
                string Key = Regex.Match(GetCookieValue, @".+?(?==)").Value;
                var data = DataAccess.Entity<TempCookie>()
                                 .FirstOrDefault(I => I.Host == Host && I.CookieKey == Key);
                if (data != null)
                {
                    data.CookieKey = Key;
                    data.CookieValue = GetCookieValue;
                    data.CreateTime = DateTime.Now;
                    data.TopTime = DateTime.Now.AddHours(24);
                }
                else
                {
                    DataAccess.Entity<TempCookie>().Add(new TempCookie
                    {
                        Host = Host,
                        CookieKey = Key,
                        CookieValue = GetCookieValue,
                        CreateTime = DateTime.Now,
                        TopTime = DateTime.Now.AddHours(24)
                    });
                }
                DataAccess.SaveChanges();
            }
        }
        private static void GetHost(ref string host)
        {
            host = HTTP.Baidu_Url[new Random().Next(0, HTTP.Baidu_Url.Count)];
            if (Tools.GetPingDelay(host) < 0)
            {
                GetHost(ref host);
            }
        }
        private static void GetUserAgent(ref string userAgent, int index = 0)
        {
            if (index != 0 && index < User_Agent.Count)
            {
                userAgent = User_Agent[index];
            }
            else
            {
                userAgent = User_Agent[new Random().Next(User_Agent.Count)];
            }
        }

        /// <summary>
        /// 批量查询百度收录状况
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public static async Task<List<string>> MultipleRequest(List<string> url, IProgress<EmployData> Progress)
        {
            string UrlTemplate = @"http://{0}/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd={1}&rsv_spt=1";
            string Host = "www.baidu.com";
            string userAgent = "";
            int Count = 0;
            WebProxy proxy = null;
            // 获取cookie，并拼接成字符串
            //string cookies = DataAccess.Entity<TempCookie>()
            //                        .Where(i => i.Host == Host && i.TopTime > i.CreateTime)
            //                        .Select(i => i.CookieValue)
            //                        .AsEnumerable()
            //                        .Aggregate(string.Empty, (s, c) => s + c);
            List<string> ErrorList = new List<string>();
            foreach (var item in url)
            {
                string status = "未收录";
                string color = "#FF2B00";
                string requestUrl = string.Format(UrlTemplate, Host, item);
                try
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        continue;
                    }
                    // 每请求3次更换一次user_agent
                    if (Count % 3 == 0)
                    {
                        GetUserAgent(ref userAgent, Count);
                    }
                    // 请求
                    await Task.Delay(new Random().Next(0, 3000));
                    var response = await HTTP.Get(requestUrl, string.Empty, userAgent, proxy);
                    if (response.StatusCode == HttpStatusCode.Found)
                    {
                        status = "需要验证";
                        ErrorList.Add(requestUrl);
                    }
                    if (response.Headers.TryGetValues("Location", out IEnumerable<string> location))
                    {
                        status = "需要验证";
                        ErrorList.Add(requestUrl);
                    }
                    // 检查响应状态是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        // 获取cookie
                        response.GetCookies(Host);
                        string Content = Content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(Content))
                        {
                            var ExistList = Regex.Matches(Content, @"(?<=mu="").*(?="")")
                                                             .Cast<Match>()
                                                             .Where(I => I.Value.Trim().Contains(item.Trim()));
                            if (ExistList.Count() > 0)
                            {
                                status = "已收录";
                                color = "#0e79b2";
                            }
                        }
                        else
                        {
                            status = "请求内容为空";
                            ErrorList.Add(item);
                        }

                    }
                    else
                    {
                        status = "请求失败";
                        ErrorList.Add(item);
                    }
                }
                catch (Exception e)
                {
                    status = e.Message;
                    ErrorList.Add(item);
                }
                Progress.Report(new EmployData()
                {
                    ID = ++Count,
                    Status = status,
                    Url = item,
                    Color = color,
                    LinkUrl = requestUrl.Trim()
                });
            }
            return ErrorList;
        }
        /// <summary>
        /// 获取快代理的ip地址，不过用不到了（有官方api）
        /// </summary>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public static async Task GetProxys(IProgress<Proxys> Progress)
        {
            for (int i = 1; i < 2; i++)
            {

                var ProxyResult = await Get($"https://www.kuaidaili.com/free/inha/{i}/");
                if (!ProxyResult.IsSuccessStatusCode)
                {
                    continue;
                }
                var HTMLResult = await ProxyResult.Content.ReadAsStringAsync();
                int StartIndex = HTMLResult.IndexOf("<tbody>");
                int EndIndex = HTMLResult.IndexOf("</tbody>") + 8;
                HTMLResult = HTMLResult.Substring(StartIndex, EndIndex - StartIndex);
                XmlDocument Xml = new XmlDocument();
                Xml.LoadXml(Tools.HTMLtoXML(HTMLResult));
                var GetXml = Xml.SelectNodes(@"(//tr/td[@data-title='IP'] | //tr/td[@data-title='PORT'])");
                for (int j = 0; j < GetXml.Count; j += 2)
                {
                    var item = new Proxys
                    {
                        IP = GetXml[j].InnerText,
                        Port = GetXml[j + 1].InnerText,
                    };
                    DataAccess.Entity<Proxys>().Add(item);
                    DataAccess.SaveChanges();
                    Progress.Report(item);
                }
            }
        }
        public static async Task<HttpResponseMessage> Get(string url, string cookies = null,string user_Agent = null, WebProxy proxy = null)
        {
            try
            {
                var headers = new Dictionary<string, string>
            {
                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
                { "Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2" },
                { "Connection", "keep-alive" },
                { "Upgrade-Insecure-Requests", "1" },
                { "Accept-Encoding", "gzip, deflate, br" }
            };

                if (!string.IsNullOrEmpty(cookies))
                {
                    headers["Cookie"] = cookies;
                }
                if (!string.IsNullOrEmpty(user_Agent))
                {
                    headers["User-Agent"] = user_Agent;
                }
                else
                {
                    headers["User-Agent"] = User_Agent[0];
                }

                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
                    UseCookies = false,
                    AllowAutoRedirect = false,
                };
                if (proxy != null)
                {
                    handler.Proxy = proxy;
                    handler.UseProxy = true;
                }

                var client = new HttpClient(handler);

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var response = await client.GetAsync(url);

                if (response.Content.Headers.ContentEncoding.Contains("br"))
                {
                    var buffer = await response.Content.ReadAsByteArrayAsync();
                    response.Content = new ByteArrayContent(Brotli.DecompressBuffer(buffer, 0, buffer.Length));
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static async Task<string> Post(this HttpClient _httpClient, string url, Dictionary<string, string> data, RequestType contentType)
        {
            HttpContent content = null;
            if (contentType == RequestType.FormData)
            {
                content = new FormUrlEncodedContent(data);
            }
            else if (contentType == RequestType.String)
            {
                var builder = new StringBuilder();
                foreach (var item in data)
                {
                    builder.Append(builder.Length == 0 ? "?" : "&");
                    builder.Append($"{item.Key}={item.Value}");
                }
                content = new StringContent(builder.ToString());
            }

            using (var result = await _httpClient.PostAsync(url, content))
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}

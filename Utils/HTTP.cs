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
using System.Diagnostics;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Windows.Media.Animation;
using System.ComponentModel;
using BeginSEO.View;

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

        public static void GetCookies(this HttpResponseMessage result, string Host, string proxyId)
        {
            IEnumerable<string> head = new List<string>();
            if (!result.Headers.TryGetValues("Set-Cookie", out head))
            {
                return;
            }
            foreach (var item in head)
            {
                string GetCookieValue = Regex.Match(item, @".*?;").Value;
                string Key = Regex.Match(GetCookieValue, @".+?(?==)").Value;
                var data = DataAccess.Entity<TempCookie>()
                                 .FirstOrDefault(I => I.Host == Host && I.CookieKey == Key && I.ProxyId == proxyId);
                if (data != null)
                {
                    data.ProxyId = proxyId;
                    data.CookieKey = Key;
                    data.CookieValue = GetCookieValue;
                    data.CreateTime = DateTime.Now;
                    data.TopTime = DateTime.Now.AddHours(24);
                }
                else
                {
                    DataAccess.Entity<TempCookie>().Add(new TempCookie
                    {
                        ProxyId = proxyId,
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
        public static string GetUserAgent(int index = 0)
        {
            string userAgent = string.Empty;
            return GetUserAgent(ref userAgent);
        }
        public static string GetUserAgent(ref string userAgent, int index = 0)
        {
            if (index != 0 && index < User_Agent.Count)
            {
                userAgent = User_Agent[index];
            }
            else
            {
                userAgent = User_Agent[new Random().Next(User_Agent.Count)];
            }
            return userAgent;
        }
        /// <summary>
        /// 对代理进行测速
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetRandomProxy(this IEnumerable<Proxys> proxy)
        {
            Proxys item = null;
            item = proxy.OrderBy(p => Guid.NewGuid())
            .FirstOrDefault();

            if (item != null)
            {
                var TestSpeed = await Tools.TestProxySpeed(item, CancellationToken.None);
                if (TestSpeed.Speed > 0)
                {
                    return $"http://{TestSpeed.IP}:{TestSpeed.Port}";
                }
                else
                {
                    return await proxy.GetRandomProxy();
                }
            }

            // 所有的代理都测试失败了
            return null;
        }
        /// <summary>
        /// 使用代理多线程请求
        /// </summary>
        public static async Task MultiplePorxyGet(List<string> Url, IProgress<(string, int, HttpResponseMessage)> Report)
        {
            int Aggregate = 0;
            var random = new Random();
            // 异步锁，只允许一个线程执行某个操作
            //SemaphoreSlim _semaphore = new SemaphoreSlim(1);
            var ProxyList = await DataAccess.Entity<Proxys>()
                                .Where(p => p.Status == ProxyStatus.Success && p.Speed > 0)
                                .ToListAsync();// Guid.NewGuid不支持翻译为Sql语句;
            // 设置最大并发线程数为 5
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 5 };

            Parallel.ForEach(Url, parallelOptions, async (item) =>
            {
                if (string.IsNullOrEmpty(item.Trim()))
                {
                    return;
                }
                string ProxyId = string.Empty;
                string userAgent = GetUserAgent(Aggregate);
                // 获取随机代理，start
                //try
                //{
                //    await _semaphore.WaitAsync();
                //}
                //finally { _semaphore.Release(); }
                ProxyId = await ProxyList.GetRandomProxy();
                WebProxy Proxy = new WebProxy(ProxyId);
                // 如果请求错误则尝试重新请求
                int RequestError = 0;
                do
                {
                    try
                    {
                        var request = await HTTP.Get(item, string.Empty, userAgent, Proxy);
                        if (!request.IsSuccessStatusCode)
                        {
                            await Task.Delay(random.Next(3000));
                            ++RequestError;
                        }
                        else
                        {
                            // 获取cookie
                            request.GetCookies(Tools.GetDomain(item) ,ProxyId);
                            Report.Report((item, Aggregate, request));
                            ++Aggregate;
                        }
                    }
                    catch (Exception)
                    {
                        ++RequestError;
                    }
                } while (RequestError > 0 && RequestError < 3);
            });
        }
        /// <summary>
        /// 批量请求
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> MultipleGet(List<string> Url, bool UseProxy, IProgress<(string, int, HttpResponseMessage)> Report)
        {
            int Aggregate = 0;
            string userAgent = string.Empty;
            string host = string.Empty;
            var random = new Random();
            IEnumerable<Proxys> ProxyList;
            if (UseProxy)
            {
                ProxyList = DataAccess.Entity<Proxys>()
                                .Where(p => p.Status == ProxyStatus.Success && p.Speed > 0)
                                .AsEnumerable();// Guid.NewGuid不支持翻译为Sql语句;
            }
            else
            {
                ProxyList = Enumerable.Empty<Proxys>();
            }

            WebProxy Proxy = null;
            foreach (var item in Url)
            {
                if (string.IsNullOrEmpty(item.Trim()))
                {
                    continue;
                }
                if (Aggregate % 3 == 0)
                {
                    GetUserAgent(ref userAgent, Aggregate);
                    if (UseProxy)
                    {
                        host = await ProxyList.GetRandomProxy();
                        Proxy = new WebProxy(host);
                    }
                }

                int RequestError = 0;
                do
                {
                    try
                    {
                        await Task.Delay(random.Next(3000));
                        var request = await HTTP.Get(item, string.Empty, userAgent, Proxy);
                        if (!request.IsSuccessStatusCode)
                        {
                            ++RequestError;
                        }
                        else
                        {
                            // 获取cookie
                            request.GetCookies(Tools.GetDomain(item), host);
                            Report.Report((item, Aggregate, request));
                            ++Aggregate;
                        }
                    }
                    catch (Exception)
                    {
                        ++RequestError;
                    }

                } while (RequestError != 0 && RequestError < 3);
            }

            return true;
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
                client.Timeout = TimeSpan.FromSeconds(10000);

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
                    builder.Append(builder.Length == 0 ? string.Empty : "&");
                    //builder.Append(builder.Length == 0 ? "?" : "&");
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

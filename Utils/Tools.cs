using BeginSEO.Data;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BeginSEO.Utils
{
    public static class Tools
    {
        public static object GetResource(string DictionaryName)
        {
            // 获取指定资源字典中的资源
            return Application.Current.Resources[DictionaryName];
        }
        public static Brush GetBrush(string Name)
        {
            return GetResource(Name) as SolidColorBrush;
            //return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }
        /// <summary>
        /// 返回一个时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(this DateTime date)
        {
            return $"{(date.ToUniversalTime().Ticks - 621355968000000000) / 10000000}";
        }
        /// <summary>
        /// 生成指定的时间范围，用于百度搜索
        /// </summary>
        /// <returns></returns>
        public static string GetBaiduTime(int LastDay = 30)
        {
            var LastTime = DateTime.Now.AddDays(LastDay);
            var query = $"stf={LastTime.GetDateTimeFormats()},{DateTime.Now.GetTimeStamp()}|stftype=1";
            return WebUtility.UrlEncode(query);
        }
        /// <summary>
        /// 检测一个地址的延迟
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>成功则返回延迟，失败返回-1</returns>
        public static int GetPingDelay(string ipAddress)
        {
            int pingDelay = -1;
            if (!string.IsNullOrEmpty(ipAddress))
            using (Ping pingSender = new Ping())
            {
                PingReply reply = pingSender.Send(ipAddress);
                if (reply.Status == IPStatus.Success)
                {
                    pingDelay = (int)reply.RoundtripTime;
                }
            }
            return pingDelay;
        }
        /// <summary>
        /// 将HTML转换为xml
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string HTMLtoXML(string HTML)
        {
            // 读取 HTML 文件
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);

            // 将 HTML 转换为有效的 XML 文档
            return htmlDoc.DocumentNode.OuterHtml;
        }
        /// <summary>
        /// 分隔IP:端口(127.0.0.1:80)
        /// </summary>
        /// <returns></returns>
        public static string[] SplitIpAndPort(string ip)
        {
            var ipAndPort = ip.Split(':');
            return ipAndPort;
        }
        public static string SerializerJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T DeSerializeJson<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }
        /// <summary>
        /// 对代理进行测速
        /// </summary>
        public static async Task<(int Speed, ProxyStatus status)> TextSpeed(string IP, string port, CancellationToken cancellationToken)
        {
            try
            {
                // 创建一个 WebRequest 对象
                WebRequest request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 10000; // 设置请求超时时间为10秒

                // 设置代理服务器
                request.Proxy = new WebProxy($"http://{IP}:{port}");

                // 测速
                //Stopwatch stopwatch = Stopwatch.StartNew();

                // 发送请求并获取响应
                using (WebResponse response = await request.GetResponseAsync().WithCancellation(cancellationToken))
                {
                    if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                    {
                        return (-1, ProxyStatus.Error);
                    }
                }
                // 输出响应时间
                //return ((int)stopwatch.ElapsedMilliseconds, ProxyStatus.Success);
                return (Tools.GetPingDelay(IP), ProxyStatus.Success);
            }
            catch (Exception ex)
            {
                // 其他未知异常
                return (-1, ProxyStatus.Error);
            }
        }
        /// <summary>
        /// 对IP进行测速
        /// </summary>
        /// <param name="proxyAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Proxys> TestProxySpeed(Proxys proxy, CancellationToken cancellationToken)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Proxy = new WebProxy($"http://{proxy.IP}:{proxy.Port}");
                using (var client = new HttpClient(handler))
                {
                    var url = "https://www.baidu.com/";
                    var stopwatch = new Stopwatch();
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // 重试次数
                        int retryCount = 2;

                        while (retryCount-- > 0)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            // 开始计时
                            stopwatch.Restart();

                            // 发送 GET 请求
                            var response = await client.GetAsync(url, cancellationToken);

                            // 停止计时
                            stopwatch.Stop();

                            // 如果响应状态码为成功，则返回响应时间
                            if (response.IsSuccessStatusCode)
                            {
                                proxy.Speed = (int)stopwatch.Elapsed.TotalMilliseconds;
                                proxy.Status = ProxyStatus.Success;
                                break;
                            }

                            // 响应状态码不成功，进行重试
                            await Task.Delay(1000, cancellationToken);
                        }
                    }
                    catch (Exception)
                    {
                        // 如果请求出现异常，则认为代理 IP 不可用
                        proxy.Status = ProxyStatus.Error;
                        proxy.Speed = -1;
                    }
                }
            }

            return proxy;
        }
        /// <summary>
        /// 批量测速
        /// </summary>
        /// <param name="proxyAddresses"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<List<Proxys>> TestProxySpeeds(List<Proxys> proxyAddresses, CancellationToken cancellationToken)
        {
            var tasks = proxyAddresses.Select(proxy => TestProxySpeed(proxy, cancellationToken));
            var responseTimes = await Task.WhenAll(tasks);

            return responseTimes.ToList();
        }

        /// <summary>
        /// UI线程内执行操作
        /// </summary>
        public static void Dispatcher(Action func)
        {
            App.Current.Dispatcher.Invoke(func);
        }
    }
}

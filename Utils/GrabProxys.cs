using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static BeginSEO.Utils.HTTP;

namespace BeginSEO.Utils
{
    public static class GrabProxys
    {
        static async Task<string> Request(string url)
        {
            try
            {
                var ProxyResult = await Get(url);
                if (!ProxyResult.IsSuccessStatusCode)
                {
                    return null;
                }
                return await ProxyResult.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                ShowToast.Show(e.Message, ShowToast.Type.Error);
                return null;
            }
        }
        /// <summary>
        /// 获取所有代理IP
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Proxys>> RequestAll()
        {
            var kuai = await GetProxys(2);
            var _89List = await Get89Proxy();
            var proxyScrape = await GetProxyScrape();


            return kuai.Concat(_89List).Concat(proxyScrape);
        }
        /// <summary>
        /// 获取快代理的ip地址
        /// </summary>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public static async Task<List<Proxys>> GetProxys(int Count)
        {
            List<Proxys> proxyList = new List<Proxys>();
            try
            {
                for (int i = 1; i < Count; i++)
                {
                    var HTMLResult = await Request($"https://www.kuaidaili.com/free/inha/{i}/");
                    if (HTMLResult == null)
                    {
                        continue;
                    }
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
                        proxyList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // 异常处理逻辑，例如记录错误日志
                throw new LoggingException(ex.Message);
            }

            return proxyList;
        }
        /// <summary>
        /// 获取89ip的代理
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static async  Task<List<Proxys>> Get89Proxy(int size = 3700)
        {
            List<Proxys> proxyList = new List<Proxys>();

            try
            {
                var proxyHtml = await Request($"http://api.89ip.cn/tqdl.html?api=1&num={size}&port=&address=&isp=");
                if (proxyHtml == null)
                {
                    // 错误处理逻辑
                    return proxyList;
                }

                var proxyMatches = Regex.Matches(proxyHtml, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}");
                foreach (Match match in proxyMatches)
                {
                    var proxy = Tools.SplitIpAndPort(match.Value);
                    var proxys = new Proxys
                    {
                        IP = proxy[0],
                        Port = proxy[1],
                        Speed = 0,
                        Status = 0,
                    };

                    proxyList.Add(proxys);
                }
            }
            catch (Exception ex)
            {
                // 异常处理逻辑，例如记录错误日志
                throw new LoggingException(ex.Message);
            }

            return proxyList;
        }
        /// <summary>
        /// 获取ProxyScrape的代理节点
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Proxys>> GetProxyScrape()
        {
            List<Proxys> proxyList = new List<Proxys>();
            var result = await Request("https://api.proxyscrape.com/proxytable.php?nf=true&country=all");

            if (result == null)
            {
                return null;
            }
            var JsonResult = JObject.Parse(result)["http"];
            foreach (var item in JsonResult)
            {
                if (item is JProperty value)
                {
                    var ipAndPort = Tools.SplitIpAndPort(value.Name);
                    if (ipAndPort != null && ipAndPort.Length > 2)
                        proxyList.Add(new Proxys
                        {
                            IP = ipAndPort[0],
                            Port = ipAndPort[1],
                        });
                }
            }

            return proxyList;
        }
    }
}

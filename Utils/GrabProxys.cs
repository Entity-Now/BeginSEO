using BeginSEO.Data;
using BeginSEO.SQL;
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
        public static async Task<ConcurrentQueue<Proxys>> RequestAll()
        {
            ConcurrentQueue<Proxys> NewProxys = new ConcurrentQueue<Proxys>();
            var ResultProxy = new Progress<Proxys>((item) =>
            {
                NewProxys.Enqueue(item);
            });
            await GetProxys(2, ResultProxy);
            await Get89Proxy(ResultProxy);
            await GetProxyScrape(ResultProxy);
            
            return NewProxys;
        }
        /// <summary>
        /// 获取快代理的ip地址
        /// </summary>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public static async Task GetProxys(int Count, IProgress<Proxys> Report)
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
                    Report.Report(item);
                }
            }
        }
        /// <summary>
        /// 获取89ip的代理
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static async Task Get89Proxy(IProgress<Proxys> Report, int size = 3700)
        {
            var ProxyHtml = await Request($"http://api.89ip.cn/tqdl.html?api=1&num={size}&port=&address=&isp=");
            if (ProxyHtml == null)
            {
                return;
            }
            var ProxyList = Regex.Matches(ProxyHtml, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}");
            foreach (Match item in ProxyList)
            {
                var proxy = Tools.SplitIpAndPort(item.Value);

                Report.Report(new Proxys
                {
                    IP = proxy[0],
                    Port = proxy[1],
                    Speed = 0,
                    Status = 0,
                });
            }
        }
        /// <summary>
        /// 获取ProxyScrape的代理节点
        /// </summary>
        /// <returns></returns>
        public static async Task GetProxyScrape(IProgress<Proxys> Report)
        {
            var result = await Request("https://api.proxyscrape.com/proxytable.php?nf=true&country=all");

            if (result == null)
            {
                return;
            }
            var JsonResult = JObject.Parse(result);
            foreach (var item in JsonResult["http"].mGetPropertys())
            {
                var ipAndPort = Tools.SplitIpAndPort(item.Name);
                Report.Report(new Proxys
                {
                    IP = ipAndPort[0],
                    Port = ipAndPort[1],
                });
            }
        }
    }
}

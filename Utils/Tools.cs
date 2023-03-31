using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
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
        public static string GetTimeStamp()
        {
            return $"{(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000}";
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
    }
}

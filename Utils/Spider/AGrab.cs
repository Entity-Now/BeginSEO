using BeginSEO.Data;
using BeginSEO.SQL;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public abstract class AGrab
    {
        public AGrab(string keyWord, int Count = 50)
        {
            this.Count = Count;
            string template = @"https://www.baidu.com/s?q3={0}&gpc={1}&q6={2}&tn=baiduadv&ie=utf-8&rn={3}&pn{4}";
            ReplaceTemplate = string.Format(template, keyWord, Tools.GetBaiduTime(), URL, Count, InPage);
        }
        public abstract string URL { get; set; }
        /// <summary>
        /// 当前页码，等于0就是第一页，如果Count=50那么将此值设为50就是第二页，之后的页面以此类推
        /// </summary>
        public virtual int InPage { get; set; } = 0;
        /// <summary>
        /// 搜索结果的数量
        /// </summary>
        public virtual int Count { get; set; }
        /// <summary>
        /// 文章截取模板
        /// </summary>
        public virtual string ReplaceTemplate { get; set; }
        public virtual async Task<List<string>> Grab(bool UseProxy)
        {
            var random = new Random();
            var ProxyList = await DataAccess.Entity<Proxys>()
                    .Where(p => p.Status == ProxyStatus.Success && p.Speed > 0)
                    .ToListAsync();// Guid.NewGuid不支持翻译为Sql语句;
            int RequestError = 0;
            do
            {
                WebProxy proxy = new WebProxy(await ProxyList.GetRandomProxy());
                var result = await HTTP.Get(ReplaceTemplate, string.Empty, HTTP.GetUserAgent(), proxy);
                if (!result.IsSuccessStatusCode)
                {
                    await Task.Delay(random.Next(3000));
                    ++RequestError;
                    continue;
                }
                var html = new HtmlDocument();
                html.LoadHtml(await result.Content.ReadAsStringAsync());
                var search = html.DocumentNode.SelectNodes("//div[contains(@class, 'result c-container')]");
                if (search.Count <= 0)
                {
                    return null;
                }
                var tempList = new List<string>();
                foreach (var item in search)
                {
                    var link = item.Attributes["mu"].Value;
                    var child = item.InnerText;
                    tempList.Add(link);
                }

            } while (RequestError > 0 && RequestError < 3);

            return null;
        }
        /// <summary>
        /// 解析抓取后的文章
        /// </summary>
        /// <returns></returns>
        public abstract Article DeSerializer(string content);
    }
}

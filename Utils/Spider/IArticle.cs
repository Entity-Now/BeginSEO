using BeginSEO.Data.DataEnum;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    /// <summary>
    /// 解析39文章库
    /// </summary>
    public abstract class IArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 每页默认有13条数据
        /// </summary>
        public virtual double inCount { get; set; } = 13;
        public virtual ArticleEnum Type { get; set; } = ArticleEnum.Null;
        public string NextPage { get; set; }

        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询页面内的文章链接
        /// </summary>
        /// <returns></returns>
        public abstract string xGetLinks(string url);
        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询文章内容
        /// </summary>
        /// <returns></returns>
        public abstract string xGetContent(string url);
        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询分页
        /// </summary>
        /// <returns></returns>
        public abstract string xGetPages(string url);
        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(List<string>, string)> DeSerializeLinks(HttpResponseMessage res)
        {
            string content = await res.Content.ReadAsStringAsync();
            string host = res.RequestMessage.RequestUri.Host;
            var links = new List<string>();
            var html = new HtmlDocument();
            html.LoadHtml(content);
            var search = html.DocumentNode.SelectNodes(xGetLinks(host));
            if (search == null || search.Count <= 0)
            {
                return (null, null);
            }
            foreach (var link in search)
            {
                links.Add(link.GetAttributeValue("href", "null"));
            }
            // 获取页码
            var findLinks = html.DocumentNode.SelectSingleNode(xGetPages(host));
            NextPage = findLinks.GetAttributeValue("href", "null");
            if (!Regex.IsMatch(@"[\w.-]+(?<=\.)\w+", NextPage))
            {
                if (NextPage.IndexOf('/') == 0)
                {
                    NextPage = $"https://{host}{NextPage}";
                }
                else
                {
                    var Path = res.RequestMessage.RequestUri.AbsoluteUri;
                    var lastIndex = Path.LastIndexOf("/");
                    NextPage = $"{Path.Substring(0, lastIndex)}/{NextPage}";
                }
            }
            return (links, NextPage);
        }
        /// <summary>
        /// 解析HTML，返回文章
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IArticle> DeSerializeArticle(HttpResponseMessage res)
        {
            string host = res.RequestMessage.RequestUri.Host;
            var html = new HtmlDocument();
            html.LoadHtml(await Tools.GetHtmlFromUrl(res, Encoding.GetEncoding("GB2312")));

            Content = html.DocumentNode.SelectNodes(xGetContent(host)).Aggregate(string.Empty, (newVal, oldVal) =>
            {
                return newVal + "\n" + oldVal.InnerText;
            });
            Title = html.DocumentNode.SelectSingleNode(@"//title").InnerText;
            Description = html.DocumentNode.SelectSingleNode(@"//meta[contains(@name, 'Description')]").
                GetAttributeValue("content", "null");

            return this;
        }
    }
}

using BeginSEO.Data.DataEnum;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public virtual ArticleEnum Type { get; set; } = ArticleEnum._39;
        public string LastPage { get; set; }
        public string NextPage { get; set; }

        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询页面内的文章链接
        /// </summary>
        /// <returns></returns>
        public abstract string xGetLinks();
        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询文章内容
        /// </summary>
        /// <returns></returns>
        public abstract string xGetContent();
        /// <summary>
        /// 此函数返回一个xpath的指令，用于查询分页
        /// </summary>
        /// <returns></returns>
        public abstract string xGetPages();
        /// <summary>
        /// 解析
        /// </summary>
        /// <returns></returns>
        public virtual List<string> DeSerializeLinks(string content)
        {
            var links = new List<string>();
            var html = new HtmlDocument();
            html.LoadHtml(content);
            var search = html.DocumentNode.SelectNodes(xGetLinks());
            if (search.Count <= 0)
            {
                return null;
            }
            foreach ( var link in search )
            {
                links.Add(link.GetAttributeValue("href","null"));
            }
            // 获取页码
            var findLinks = html.DocumentNode.SelectNodes(xGetPages());
            LastPage = findLinks[0].InnerText;
            NextPage = findLinks[1].InnerText;
            return links;
        }
        /// <summary>
        /// 解析HTML，返回文章
        /// </summary>
        /// <returns></returns>
        public virtual IArticle DeSerializeArticle(string content)
        {
            var html = new HtmlDocument();
            html.LoadHtml(content);
            Content = html.DocumentNode.SelectNodes(xGetContent()).Aggregate(string.Empty, (newVal, oldVal) =>
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

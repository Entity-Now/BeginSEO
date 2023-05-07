﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public class _39Article : IArticle
    {

        public override string xGetContent(string url)
        {
            return getType(url, Types.Content);
        }

        public override string xGetLinks(string url)
        {
            return getType(url, Types.Links);
        }

        public override string xGetPages(string url)
        {
            return getType(url, Types.Pages);
        }
        public enum Types
        {
            Content,
            Links,
            Pages
        }
        public string getType(string url, Types type)
        {
            Dictionary<string, string> ContentTemplate = new Dictionary<string, string>()
            {
                {"jianfei.fh21.com.cn","article-content" },
                {"yyk.fh21.com.cn","article-content" },
                {"news.fh21.com.cn","article-content" },
                {"disease.39.net","page4txt" },
                {"cm.39.net","art_con" },
            };
            List<string> LinksTemplate = new List<string>()
            {
                // 39
                @"//ul[@class='{0}']/li/*/a/@href",
                // 复合的
                @"//div[contains(@class, 'tsyl-list')]/div/div/a/@href"
            };
            List<string> PageTemplate = new List<string>()
            {
                // 39
                @"//p[contains(@class, '{0}')]/*[(position() = last() - 1)]/a/@href",
                // 复合
                @"//div[contains(@class, 'page-main')]/li[(position() = last() - 1)]/a/@href"
            };
            // Key的三个参数分别是 模板索引、文章列表、分页列表
            Dictionary<string, (int, string, string)> LinksStation = new Dictionary<string, (int, string, string)>()
            {
                {"jianfei.fh21.com.cn",(1, "left fll", "page-main") },
                {"yyk.fh21.com.cn",(1, "tsyl-list", "page-main") },
                {"news.fh21.com.cn",(1, "left fll" , "page-main")},
                {"disease.39.net",(0, "pclist", "docFlip") },
                {"yyk.39.net",(0, "newslist", "pageno") },
            };
            if (type == Types.Links)
            {
                var TemplateIndex = LinksStation[url].Item1;
                var StationSymbol = LinksStation[url].Item2;
                return string.Format(LinksTemplate[TemplateIndex], StationSymbol);
            }else if (type == Types.Pages)
            {
                var TemplateIndex = LinksStation[url].Item1;
                var StationSymbol = LinksStation[url].Item3;
                return string.Format(PageTemplate[TemplateIndex], StationSymbol);
            }else
            {
                string template = @"//div[contains(@class, '{0}')]/*[position() < last()]";
                return string.Format(template, ContentTemplate[url]);
            }
        }
    }
}

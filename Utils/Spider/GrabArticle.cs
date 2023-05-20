using BeginSEO.Data;
using BeginSEO.SQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static BeginSEO.Utils.HTTP;

namespace BeginSEO.Utils.Spider
{
    public class GrabArticle
    {
        public string Link { get; set; }
        public List<string> ArticleList = new List<string>();
        public IArticle Article { get; set; }
        public readonly dataBank Db;
        /// <summary>
        /// 抓取数量
        /// </summary>
        public double GrabCount { get;set; }
        public GrabArticle(dataBank _Db, IArticle article, string _link, double _GrabCount) 
        {
            this.Db = _Db;
            this.Link = _link;
            this.GrabCount = _GrabCount;
            this.Article = article;
        }
        public async Task<List<string>> Grab(bool IsUseProxy)
        {
            var random = new Random();
            var ProxyList = await Db.Set<Proxys>()
                    .Where(p => p.Status == ProxyStatus.Success && p.Speed > 0)
                    .ToListAsync();// Guid.NewGuid不支持翻译为Sql语句;
            
            int RequestError = 0;
            int unfinished = 1;
            do
            {
                WebProxy proxy = !IsUseProxy ? null : new WebProxy(await ProxyList.GetRandomProxy());
                var result = await Get(this.Link, string.Empty, GetUserAgent(), proxy);
                if (!result.IsSuccessStatusCode)
                {
                    await Task.Delay(random.Next(3000));
                    ++RequestError;
                    continue;
                }
                // 获取所有文章数量
                var (Links, NextPage) = await Article.DeSerializeLinks(result);
                ArticleList.AddRange(Links);

                //int residue = (int)Math.Ceiling((double)(GrabCount / (Article.inCount * unfinished)));
                if (unfinished >= GrabCount)
                {
                    await Task.Delay(random.Next(3000));
                    this.Link = NextPage;
                    ++unfinished;
                }
                else
                {
                    unfinished = 1;
                }

            } while ((RequestError > 0 && RequestError < 3) || unfinished > 1);
            // 判断数据库里面是否已存在改文章，一篇文章不抓取第二次
            foreach (var item in ArticleList.ToList())
            {
                var data = Db.Set<Article>().FirstOrDefault(I => I.Url == item);
                if (data != null)
                {
                    ArticleList.Remove(item);
                }
            }
            // 去重
            ArticleList = ArticleList.Distinct().ToList();
            return ArticleList;
        }
        public async void GrabArticles(List<string> Links, bool IsUseProxy)
        {
            Tools.Dispatcher(ShowModal.ShowLoading);
            var func = new Progress<(string, int, HttpResponseMessage)>(async val =>
            {
                var (url, aggregate, res) = val;

                if (!res.IsSuccessStatusCode)
                {
                    await ShowToast.Show($"请求失败：{url}~",ShowToast.Type.Error);
                    return;
                }
                var result = await Article.DeSerializeArticle(res);
                Db.Set<Article>().Add(new Data.Article
                {
                    Content = result.Content,
                    GrabTime = DateTime.Now,
                    IsInspect = false,
                    Title = result.Title,
                    Type = Article.Type,
                    Url = url,
                    IsUse = false,
                    IsUseReplaceKeyword = false,
                    IsUseRewrite = false,
                    Contrast = 0,
                    Rewrite = string.Empty
                });
                await Db.SaveChangesAsync();
            });
            if (IsUseProxy)
            {
                await MultiplePorxyGet(Links ?? ArticleList, func);
            }
            else
            {
                await MultipleGet(Links ?? ArticleList, IsUseProxy, func);
            }
            Tools.Dispatcher(ShowModal.Closing);
        }
    }
}

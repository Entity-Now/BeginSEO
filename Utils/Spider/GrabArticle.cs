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
using static BeginSEO.Utils.HTTP;

namespace BeginSEO.Utils.Spider
{
    public class GrabArticle
    {
        public string Link { get; set; }
        public List<string> ArticleList = new List<string>();
        public IArticle Article { get; set; }
        /// <summary>
        /// 抓取数量
        /// </summary>
        public double GrabCount { get;set; }
        public GrabArticle(IArticle article, string _link, double _GrabCount) 
        {
            this.Link = _link;
            this.GrabCount = _GrabCount;
            this.Article = article;
        }
        public async Task<List<string>> Grab(bool IsUseProxy)
        {
            var random = new Random();
            var ProxyList = await DataAccess.Entity<Proxys>()
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
                string content = await result.Content.ReadAsStringAsync();
                // 获取所有文章数量
                var Links = Article.DeSerializeLinks(content);
                ArticleList.AddRange(Links);

                int residue = (int)Math.Ceiling((double)(GrabCount / (Article.inCount * unfinished)));
                if (residue != 1)
                {
                    await Task.Delay(random.Next(3000));
                    this.Link = Article.NextPage;
                    ++unfinished;
                }

            } while ((RequestError > 0 && RequestError < 3) || unfinished > 1);

            return ArticleList;
        }
        public async void GrabArticles(List<string> Links, bool IsUseProxy)
        {
            var func = new Progress<(string, int, HttpResponseMessage)>(async val =>
            {
                var (url, aggregate, res) = val;
                if (!res.IsSuccessStatusCode)
                {
                    await ShowToast.Show($"请求失败：{url}~",ShowToast.Type.Error);
                    return;
                }
                string Content = await res.Content.ReadAsStringAsync();
                var result = Article.DeSerializeArticle(Content);
                DataAccess.Entity<Article>().Add(new Data.Article
                {
                    Content = result.Content,
                    GrabTime = DateTime.Now,
                    IsInspect = false,
                    Title = result.Title,
                    Type = Article.Type,
                    Url = url
                });
                await DataAccess.BeginContext.SaveChangesAsync();
            });
            if (IsUseProxy)
            {
                await MultiplePorxyGet(Links ?? ArticleList, func);
            }
            else
            {
                await MultipleGet(Links ?? ArticleList, IsUseProxy, func);
            }
        }
    }
}

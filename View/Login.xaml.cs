using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeginSEO.View
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            webView.NavigationStarting += NavigationStaring;
            webView.NavigationCompleted += NavigationCompleted;
            webView.CoreWebView2InitializationCompleted += WebViewInitialization;
        }
        /// <summary>
        /// 获取Cookies
        /// </summary>
        public async void GetCookies()
        {
            // 获取URL地址
            var currentUrl = webView.CoreWebView2.Source;
            var currentUri = new Uri(currentUrl);
            var cookieList = await webView.CoreWebView2.CookieManager.GetCookiesAsync(currentUrl);

            foreach (var cookie in cookieList)
            {
                DataAccess.InsertOrUpdate(new TempCookie
                {
                    Host = currentUri.Host,
                    CookieKey = cookie.Name,
                    CookieValue = cookie.Value,
                    Domain = cookie.Domain,
                    Path = cookie.Path,
                    CreateTime = DateTime.Now,
                },I=> I.Host == currentUrl && I.CookieKey == cookie.Name);
            }

        }
        /// <summary>
        /// 导航事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void NavigationStaring(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            Uri uri = new Uri(e.Uri);
            var Headers = e.RequestHeaders.Select(I => new Header
            {
                Name = I.Key,
                Value = I.Value
            }).ToList();
            foreach (var item in Headers)
            {
                item.Host = uri.Host;
                item.Domain = uri.AbsolutePath;
                DataAccess.InsertOrUpdate(item, I => I.Domain == uri.AbsolutePath || I.Host == uri.Host);
            }
            var HeaderList = DataAccess.Entity<Header>().Where(I => I.Domain == uri.Host);
            foreach (var item in HeaderList)
            {
                if (Headers.FirstOrDefault(I => I.Name == item.Name) == null)
                {
                    e.RequestHeaders.SetHeader(item.Name, item.Value);
                }
            }
        }
        /// <summary>
        /// 导航结束，获取请求头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                // 获取 CoreWebView2 实例
                var webView = (WebView2)sender;
                // 获取URL地址
                string currentUrl = currentUrl = webView.CoreWebView2.Source;
                var currentUri = new Uri(currentUrl);

                // 执行 JavaScript 代码获取请求头
                var script = "JSON.stringify(Object.fromEntries([...new Headers(navigator.userAgentHeaders || {})].map(([name, value]) => [name, value])))";
                var result = await webView.ExecuteScriptAsync(script);
                var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                // 处理请求头
                // ...
                foreach (var item in headers)
                {
                    DataAccess.InsertOrUpdate(new Header
                    {
                        Domain = currentUrl,
                        Host = currentUri.Host,
                        Name = item.Key,
                        Value = item.Value
                    }, I => I.Host == currentUri.Host);
                }
            }
            catch (Exception)
            {

            }

        }
        public void WebViewInitialization(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // WebView2控件初始化成功
                GetHead();
            }
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        public async void SetCookies()
        {
            // 获取URL地址
            var currentUrl = webView.CoreWebView2.Source;
            var currentUri = new Uri(currentUrl);
            var CookieHandle = webView.CoreWebView2.CookieManager;
            var data = DataAccess.Entity<TempCookie>().Where(I=>I.Host == currentUri.Host);
            foreach (var item in data)
            {
               var cookie = CookieHandle.CreateCookie(item.CookieKey, item.CookieValue, item.Domain, item.Path);
                CookieHandle.AddOrUpdateCookie(cookie);
            }
        }
        /// <summary>
        /// 获取请求头
        /// </summary>
        public void GetHead()
        {
            // 获取URL地址
            var currentUrl = webView.CoreWebView2.Source;
            var currentUri = new Uri(currentUrl);
            //webView.CoreWebView2.RemoveWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.CoreWebView2.WebResourceRequested += (sender, args) =>
            {
                var Headers = args.Request.Headers.Select(I=>new Header
                {
                    Name = I.Key,
                    Value = I.Value
                }).ToList();
                foreach (var item in Headers)
                {
                    item.Host = currentUri.Host;
                    item.Domain = currentUrl;
                    DataAccess.InsertOrUpdate(item, I => I.Domain == currentUrl || I.Host == currentUri.Host);
                }
                var HeaderList = DataAccess.Entity<Header>().Where(I => I.Domain == currentUrl || I.Host == currentUri.Host);
                foreach (var item in HeaderList)
                {
                    if (Headers.FirstOrDefault(I=> I.Name == item.Name) == null)
                    {
                        args.Request.Headers.Append(new KeyValuePair<string, string>(item.Name, item.Value));
                    }
                }
            };
        }
    }
}

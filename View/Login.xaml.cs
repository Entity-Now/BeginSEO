using BeginSEO.Data;
using BeginSEO.SQL;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        }
        /// <summary>
        /// 设置请求头
        /// </summary>
        public void SetAuthorizaiton()
        {
            
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
        /// 设置Cookies
        /// </summary>
        public async void SetCookies()
        {
            // 获取URL地址
            var currentUrl = webView.CoreWebView2.Source;
            var currentUri = new Uri(currentUrl);
            var data = DataAccess.Entity<TempCookie>().Where(I=>I.Host == currentUri.Host);
            foreach (var cookie in data)
            {

            }
        }
    }
}

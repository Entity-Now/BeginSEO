using BeginSEO.Attributes;
using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Threading;

namespace BeginSEO.View
{
    /// <summary>
    /// ProxyList.xaml 的交互逻辑
    /// </summary>
    [Pages("网络代理")]
    public partial class ProxyList : UserControl
    {
        CollectionViewSource ViewProxyList;
        public ProxyList()
        {
            InitializeComponent();
            ViewProxyList = (CollectionViewSource)FindResource(nameof(ViewProxyList));
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            DataAccess.Entity<Proxys>().Load();
            ViewProxyList.Source = DataAccess.Entity<Proxys>().Local.ToObservableCollection();
        }
        /// <summary>
        /// 获取网络IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrabProxy_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(ShowModal.ShowLoading);
            new Thread(() =>
            {
                var result = HTTP.Get89Proxy(new Progress<Proxys>((res) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var IsExist = DataAccess.Entity<Proxys>().FirstOrDefault(I => I.IP == res.IP && I.Port == res.Port);
                        if (IsExist == null)
                        {
                            DataAccess.Entity<Proxys>().Add(res);
                        }

                    });
                }), new Progress<bool>((res) =>
                {
                    Dispatcher.Invoke(ShowModal.Closing);
                    DataAccess.SaveChanges();
                }));
            }).Start();
        }
    }
}

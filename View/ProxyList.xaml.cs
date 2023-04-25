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
using NPOI.Util;

namespace BeginSEO.View
{
    /// <summary>
    /// ProxyList.xaml 的交互逻辑
    /// </summary>
    [Pages("网络代理")]
    public partial class ProxyList : UserControl
    {
        CollectionViewSource ViewProxyList;
        public ObservableCollection<Proxys> _ProxyList
        {
            get { return ((ObservableCollection<Proxys>)ViewProxyList.Source); }
            set
            {
                ViewProxyList.Source = value;
            }
        }
        public ProxyList()
        {
            InitializeComponent();
            ViewProxyList = (CollectionViewSource)FindResource(nameof(ViewProxyList));
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            DataAccess.Entity<Proxys>().Where(I=>I.Status == ProxyStatus.Success).Load();
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
        /// <summary>
        /// 对节点测速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TestSpeed_Click(object sender, RoutedEventArgs e)
        {
            Tools.Dispatcher(() => ShowModal.ShowLoading());
            var ProxyLists = new List<Task>();
            var data = DataAccess.Entity<Proxys>().Where(I=> I.Status == ProxyStatus.Success);
            // 限制并发任务的数量
            //SemaphoreSlim semaphore = new SemaphoreSlim(10);
            foreach (var item in data)
            {
                ProxyLists.Add(Task.Run(async () =>
                {
                    /// 10秒后强制暂停测速
                    var canCell = new CancellationTokenSource(10000);
                    var (speed, status) = await Tools.TextSpeed(item.IP, item.Port, canCell.Token);
                    item.Status = status; 
                    item.Speed = speed;
                }));
            }
            await Task.WhenAll(ProxyLists);
            await DataAccess.BeginContext.SaveChangesAsync();
            Tools.Dispatcher(() => ShowModal.Closing());
        }
    }
}

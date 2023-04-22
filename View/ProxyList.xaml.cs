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
        private void TestSpeed_Click(object sender, RoutedEventArgs e)
        {
            var delayExecution = Throttle.DelayExecution(3000);
            for (int i = 0; i < 10; i++)
            {
                delayExecution(() =>
                {
                });
            }
            //Parallel.ForEach(_ProxyList, async (item) =>
            //{
            //    var (speed, status) = await Tools.TextSpeed(item.IP, item.Port);
            //    delayExecution(() =>{
            //        Dispatcher.Invoke(() =>
            //        {
            //            var data = _ProxyList.FirstOrDefault(I => I.IP == item.IP && I.Port == item.Port);
            //            data.Speed = speed;
            //            data.Status = status;
            //            DataAccess.SaveChanges();
            //        });
            //    });

            //});
        }
    }
}

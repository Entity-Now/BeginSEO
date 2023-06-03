using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BeginSEO.ModelView
{
    public class ProxyListViewModel : ObservableObject
    {
        public readonly dataBank Db;
        public ProxyListViewModel(dataBank db) 
        {
            Db = db;
            GrabProxysCommand = new AsyncRelayCommand(GrabProxysHandle);
            TestSpeedProxyCommand = new RelayCommand(TestSpeedProxy);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            RefreshCommand = new RelayCommand(() => load());
            load();
        }
        void load()
        {
            Db.Set<Proxys>().Where(I => I.Status == ProxyStatus.Success)
                .Load();
            ProxyList = Db.Set<Proxys>().Local.ToObservableCollection();
        }
        public bool _IsUseProxy;
        public bool IsUseProxy
        {
            get => _IsUseProxy;
            set
            {
                SetProperty(ref _IsUseProxy, value);
            }
        }
        public ObservableCollection<Proxys> _ProxyList;
        public ObservableCollection<Proxys> ProxyList
        {
            get => _ProxyList;
            set
            {
                SetProperty(ref _ProxyList, value);
            }
        }
        public ICommand GrabProxysCommand { get; set; }
        public async Task GrabProxysHandle()
        {
            ShowModal.ShowLoading();
            var proxyList = await GrabProxys.RequestAll();
            Proxys proxys = null;
            while (proxyList.TryDequeue(out proxys))
            {
                var findData = await Db.Set<Proxys>().FirstOrDefaultAsync(I => I.IP == proxys.IP && I.Port == proxys.Port);
                if (findData != null)
                {
                    continue;
                }
                Db.Set<Proxys>().Add(proxys);
            }
            await Db.SaveChangesAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                ShowToast.Show($"成功获取{ProxyList.Count}个IP代理..");
                ShowModal.Closing();
            });
        }
        public ICommand TestSpeedProxyCommand { get; set; }
        public async void TestSpeedProxy()
        {
            await ShowToast.Show("测速时请勿使用代理，否则会影响准确性.", ShowToast.Type.Info);
            ShowModal.ShowLoading();
            var data = Db.Set<Proxys>().Where(I => (IsUseProxy == true || I.Status == ProxyStatus.Success));
            // 限制并发任务的数量
            var CancellToken = new CancellationTokenSource();
            var testSpeed = await Tools.TestProxySpeeds(data.ToList(), CancellToken.Token);
            foreach (var item in testSpeed)
            {
                var findProxy = data.First(I => I.IP == item.IP && I.Port == item.Port);
                findProxy.Status = item.Status;
                findProxy.Speed = item.Speed;
            }
            await Db.SaveChangesAsync();
            Tools.Dispatcher(() => ShowModal.Closing());
        }
        public ICommand RemoveAllCommand { get; set; }
        public void RemoveAll()
        {
            var entity = Db.Set<Proxys>();
            foreach (var item in entity)
            {
                entity.Remove(item);
            }
            Db.SaveChanges();
        }

        public ICommand RefreshCommand { get; set; }
    }
}

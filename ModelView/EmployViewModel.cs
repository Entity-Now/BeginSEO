using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeginSEO.Model;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using BeginSEO.Utils;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Microsoft.Win32;
using BeginSEO.Data;
using System.IO.Compression;
using System.IO;
using BrotliSharpLib;
using System.Windows.Threading;
using System.Windows.Controls;
using BeginSEO.SQL;
using System.Threading;

namespace BeginSEO.ModelView
{
    public class EmployViewModel: ObservableObject
    {
        public EmployViewModel()
        {
            Handle = new RelayCommand(GetEmploy);
            ClearList = new RelayCommand(Clear);
            OpenExcel = new RelayCommand(OpenE);
            CloseExcel = new RelayCommand(CloseE);
            CommandShowEmploy = new RelayCommand(ShowEmploy);
            CommandRemove = new RelayCommand<EmployData>(Remove);
            CommandCopy = new RelayCommand<EmployData>(Copy);
            CommandCopyExcel = new RelayCommand<EmployData>(CopyExcel);
        }
        private ObservableCollection<EmployData> _EmployList = new ObservableCollection<EmployData>();
        public ObservableCollection<EmployData> EmployList
        {
            get => _EmployList;
            set
            {
                SetProperty(ref _EmployList, value);
            }
        }
        private string _UrlList;
        public string UrlList
        {
            get => _UrlList;
            set
            {
                SetProperty(ref _UrlList, value);
            }
        }
        public ICommand Handle { get; set; }
        public ICommand ClearList { get; set; }
        public void Clear()
        {
            // 清空列表
            EmployList.Clear();
        }
        void GetEmploy() {
            ShowModal.ShowLoading();
            // 清空列表
            Clear();
            if (string.IsNullOrEmpty(UrlList))
            {
                return;
            }
            // 分割地址
            List<string> urlList = UrlList.Split('\r').ToList();

            new Thread(async () => {
                await HTTP.MultipleRequest(urlList, new Progress<EmployData>(I =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        EmployList.Add(I);
                    });
                }));
            }).Start();
            ShowModal.Closing();
        }
        public ICommand OpenExcel { get; set; }
        void OpenE()
        {
            List<ExcelEmploy> data = new List<ExcelEmploy>();

            var OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == true)
            {
                new ExcelUtils<ExcelEmploy>(OpenFile.FileName).GetHeads();
                //ExcelUtils.ImportToList(OpenFile.FileName);
            }
            foreach (var item in data)
            {
                UrlList += item.Link + "\n";
            }
        }
        public ICommand CloseExcel { get; set; }
        async void CloseE()
        {
            //ExcelUtils.ImportToList();
            var content = File.ReadAllLines(@"C:\Users\Administrator\Downloads\http_proxies.txt");
            foreach (var item in content)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                var split = Tools.SplitIpAndPort(item);
                DataAccess.Inser<Proxys>(new Proxys
                {
                    IP = split[0],
                    Popt = split[1]
                });
            }

        }
        public ICommand CommandRemove { get; set; }
        public void Remove(EmployData listViewItem)
        {
            EmployList.Remove(listViewItem);
            ShowToast.Open("删除成功");
        }
        public ICommand CommandCopy { get; set; }
        public void Copy(EmployData Item)
        {
            Clipboard.SetText(Item.Url.Trim());
            ShowToast.Open("复制成功");
        }
        public ICommand CommandCopyExcel { get; set; }
        public void CopyExcel(EmployData Item)
        {
            Clipboard.SetText($"{Item.Url} {Item.Status}");
            ShowToast.Open("复制成功");
        }
        /// <summary>
        /// 只显示已收录的链接
        /// </summary>
        public ICommand CommandShowEmploy { get; set; }
        public void ShowEmploy()
        {
            var data = EmployList
                .Where(I => I.Status == "未收录" || I.Status == "请求失败" || I.Status == "需要验证")
                .ToList();
            foreach (var item in data)
            {
                EmployList.Remove(item);
            }

        }
    }
}

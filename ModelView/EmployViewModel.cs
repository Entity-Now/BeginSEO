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
        async void GetEmploy() {
            // 清空列表
            Clear();
            if (string.IsNullOrEmpty(UrlList))
            {
                return;
            }
            // 序号
            int Count = 0;
            // 分割地址
            string[] urlList = UrlList.Split('\r');
            List<Cookie> Cookie = new List<Cookie>();
            foreach (string url in urlList)
            {
                string status = "未收录";
                string color = "#FF2B00";
                var FilterUrl = Regex.Match(url.Trim(), @"(?<=https?:\/\/|)([\w\-\.]+)\.([a-z]+)(\/[\w\-\.%\/]*)?")
                    .Value
                    .Trim();
                if (string.IsNullOrEmpty(FilterUrl))
                {
                    continue;
                }
                var Host = HTTP.Baidu_Url[new Random().Next(0, HTTP.Baidu_Url.Count)];
                string requestUrl = $"http://{Host}/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd={FilterUrl}&rsv_spt=1";
                CookieContainer CookieJar = new CookieContainer();
                var response = await HTTP.GetBaiDu(requestUrl, CookieJar, Cookie);
                // 检查响应状态是否成功
                if (response.IsSuccessStatusCode)
                {
                    string Content = Content = await response.Content.ReadAsStringAsync();

                    // 获取cookie
                    CookieJar.Replace(requestUrl.Trim(), ref Cookie);
                    if (!string.IsNullOrEmpty(Content))
                    {

                        if (response.Headers.TryGetValues("Location", out IEnumerable<string> location))
                        {
                            status = "需要验证";
                        }
                        if (response.StatusCode == HttpStatusCode.Found)
                        {
                            status = "需要验证";
                        }
                        MatchCollection ExistList = Regex.Matches(Content, @"(?<=mu="").*(?="")");
                        foreach (Match Exist in ExistList)
                        {

                            if (Exist.Value.Trim().Equals(url.Trim()))
                            {
                                status = "已收录";
                                color = "#0e79b2";
                                break;
                            }
                        }
                    }

                }
                else
                {
                    status = "请求失败";
                }
                App.Current.Dispatcher.Invoke(new Action(() => {
                    EmployList.Add(new EmployData()
                    {
                        ID = ++Count,
                        Status = status,
                        Url = url,
                        Color = color,
                        LinkUrl = url.Trim()
                    });
                }));
            }

        }
        public ICommand OpenExcel { get; set; }
        void OpenE()
        {
            List<ExcelEmploy> data = new List<ExcelEmploy>();

            var OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == true)
            {
                ExcelUtils.OpenExcel<ExcelEmploy>(OpenFile.FileName, data, new ExcelEmploy() { Split = true});
            }
            foreach (var item in data)
            {
                UrlList += item.Link + "\n";
            }
        }
        public ICommand CloseExcel { get; set; }
        void CloseE()
        {
            ShowToast.Open("test");
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

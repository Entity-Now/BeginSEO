using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 替换关键词.Model;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using 替换关键词.Utils;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Microsoft.Win32;
using 替换关键词.Data;
using System.IO.Compression;
using System.IO;
using BrotliSharpLib;
using System.Windows.Threading;

namespace 替换关键词.ModelView
{
    public class EmployViewModel: ObservableObject
    {
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
        public EmployViewModel()
        {
            Handle = new RelayCommand(GetEmploy);
            ClearList = new RelayCommand(Clear);
            OpenExcel = new RelayCommand(OpenE);
            CloseExcel = new RelayCommand(CloseE);
        }
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
            // 分割地址
            string[] urlList = UrlList.Split('\n');
            List<Cookie> Cookie = new List<Cookie>();
            // 序号
            int Count = 1;
            foreach (string url in urlList)
            {
                // 搞点延迟，避免速度太快被检测
                await Task.Delay(1500);
                if (string.IsNullOrEmpty(url.Trim()))
                {
                    continue;
                }
                var FilterUrl = Regex.Match(url.Trim(), @"(?<=(http(s?)://)).*")
                    .Value
                    .Trim();
                //string requestUrl = $"https://www.baidu.com/s?wd={FilterUrl}&rsv_spt=1"; // rsv_spt第几页
                string requestUrl = $"https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd={FilterUrl}&rsv_spt=1";
                CookieContainer CookieJar = new CookieContainer();
                var response = await HTTP.GetBaiDu(requestUrl, CookieJar, Cookie);
                // 检查响应状态是否成功
                if (response.IsSuccessStatusCode)
                {
                    string Content = "";
                    // 获取响应内容的流
                    var stream = await response.Content.ReadAsStreamAsync();

                    // 检查响应头是否包含GZIP编码
                    if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                    {
                        // 如果是GZIP编码，则创建一个GZipStream对象来解压缩流
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                        // 读取流中的数据，并转换为字符串
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            Content = await reader.ReadToEndAsync();

                        }
                    }else if (response.Content.Headers.ContentEncoding.Contains("br"))
                    {
                        // 响应头是br编码
                        byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                        // 解码
                        var c = new ByteArrayContent(Brotli.DecompressBuffer(buffer, 0, buffer.Length));
                        Content = await c.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        // 普通类型
                        Content = await response.Content.ReadAsStringAsync();
                    }
                    // 获取cookie
                    CookieJar.Replace(requestUrl.Trim(), ref Cookie);
                    if (!string.IsNullOrEmpty(Content))
                    {
                        string status = "未收录";
                        string color = "#FF2B00";
                        if (response.Headers.TryGetValues("Location", out IEnumerable<string> location))
                        {
                            status = "请求失败";
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
        public ICommand CloseExcel { get; }
        void CloseE()
        {

        }
    }
}

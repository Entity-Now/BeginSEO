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
        }
        public ICommand ClearList { get; set; }
        public void Clear()
        {
            // 清空列表
            EmployList.Clear();
        }
        async void GetEmploy() {
            Clear();
            string[] urlList = UrlList.Split('\n');
            string Cookie = "";
            int Count = 1;
            foreach (string url in urlList)
            {
                await Task.Delay(1500);
                if (string.IsNullOrEmpty(url.Trim()))
                {
                    continue;
                }
                var FilterUrl = Regex.Match(url.Trim(), @"(?<=(http(s?)://)).*");
                var result = await HTTP.GetBaiDu(FilterUrl.Value.Trim(), Cookie);
                // 获取cookie
                if (string.IsNullOrEmpty(Cookie))
                {
                    Cookie = HTTP.Cookie(result);
                }
                else
                {
                    HTTP.Replace(result,ref Cookie);
                }
                var resultHtml = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(resultHtml)) {
                    string status = "未收录";
                    string color = "#FF2B00";
                    if (result.Headers.TryGetValues("Location", out IEnumerable<string> location))
                    {
                        status = "请求失败";
                    }
                    MatchCollection ExistList = Regex.Matches(resultHtml, @"(?<=mu="").*(?="")");
                    foreach (Match Exist in ExistList) {

                        if (Exist.Value.Trim().Equals(url.Trim())) {
                            status = "已收录";
                            color = "#0e79b2";
                            break;
                        }
                    }
                    EmployList.Add(new EmployData() {
                        ID = ++Count,
                        Status = status,
                        Url = url,
                        Color = color,
                        LinkUrl = url.Trim()
                    });
                    //UrlList = result;
                }
            }

        }
    }
}

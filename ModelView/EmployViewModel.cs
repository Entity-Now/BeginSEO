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
        }
        async void GetEmploy() {
            string[] urlList = UrlList.Split('\n');
            foreach (string url in urlList)
            { 
                var temp_url = WebUtility.UrlEncode(url);
                string requestUrl = $"https://www.baidu.com/s?wd={temp_url}&rsv_spt=1";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.41");
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                client.DefaultRequestHeaders.Add("Host", "www.baidu.com");
                string result = await HTTP.Get(client, requestUrl);
                if (!string.IsNullOrEmpty(result)) {
                    string status = "未收录";
                    MatchCollection ExistList = Regex.Matches(result, @"(?<=mu="").*(?="")");
                    foreach (Match Exist in ExistList) {
                        var Employ = Regex.Match(Exist.Value.Trim(), @"(?<=http.?://).*");
                        if (!Employ.Success) {
                            continue;
                        }
                        var temp = Regex.Match(url, @"(?<=http.?://).*");
                        if (Employ.Value.Equals(temp.Value.Trim())) {
                            status = "已收录";
                            break;
                        }
                    }
                    EmployList.Add(new EmployData() {
                        Status = status,
                        Url = url
                    });
                }
            }

        }
    }
}

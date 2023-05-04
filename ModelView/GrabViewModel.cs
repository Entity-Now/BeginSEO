using BeginSEO.Data;
using BeginSEO.Utils;
using BeginSEO.Utils.Spider;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BeginSEO.ModelView
{
    public class GrabViewModel : ObservableObject
    {
        public GrabViewModel() 
        {
            GrabCommand = new RelayCommand(async () =>
            {
                if (string.IsNullOrEmpty(GrabLink))
                {
                    await ShowToast.Show("请输入要抓取的页面~", ShowToast.Type.Error);
                    return;
                }
                var _39Grab = new _39Article();
                var run = new GrabArticle(_39Grab, GrabLink, GrabCount);
                var result = await run.Grab(IsUseProxy);
                // 
                run.GrabArticles(result, IsUseProxy);
            });
        }
        public ICommand GrabCommand { get; set; }
        public List<string> _Types = new List<string> { "39疾病","全名健康网"};
        public List<string> Tpes
        {
            get => _Types;
            set 
            {
                _Types = value;
                SetProperty(ref _Types, value);
            }
        }
        public bool _IsUseProxy;
        public bool IsUseProxy
        {
            get => _IsUseProxy;
            set
            {
                _IsUseProxy = value;
                SetProperty(ref _IsUseProxy, value);
            }
        }
        public int _GrabCount = 10;
        public int GrabCount
        {
            get => _GrabCount;
            set
            {
                _GrabCount = value;
                SetProperty(ref _GrabCount, value);
            }
        }
        public string _GrabLink;
        public string GrabLink
        {
            get => _GrabLink;
            set
            {
                _GrabLink = value;
                SetProperty(ref _GrabLink, value);
            }
        }
        public ObservableCollection<Article> _GrabList;
        public ObservableCollection<Article> GrabList
        {
            get => _GrabList; 
            set
            {
                _GrabList = value;
                SetProperty(ref _GrabList, value);
            }
        }
        public ICollectionView _ArticleData;
        public ICollectionView ArticleData
        {
            get => _ArticleData; 
            set
            {
                _ArticleData = value;
                SetProperty(ref _ArticleData, value);
            }
        }
    }
}

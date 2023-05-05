using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using BeginSEO.Utils.Spider;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BeginSEO.ModelView
{
    public class GrabViewModel : ObservableObject
    {
        public GrabViewModel() 
        {
            GrabCommand = new RelayCommand(GrabHandle);
            RemoveAllCommand = new RelayCommand(RemoveAllHandle);
            // load data
            LoadData();
        }
        /// <summary>
        ///  抓取文章
        /// </summary>
        public ICommand GrabCommand { get; set; }
        public async void GrabHandle()
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
        }
        public ICommand RemoveAllCommand { get; set; }
        public void RemoveAllHandle()
        {
            ShowModal.Show("提示", "这将删除所有文章，请三四~", async (res) =>
            {
                if (res)
                {
                    var data = DataAccess.Entity<Article>().ToArray();
                    foreach (var item in data)
                    {
                        DataAccess.Entity<Article>().Remove(item);
                    }
                    await DataAccess.BeginContext.SaveChangesAsync();
                    await ShowToast.Show("已删除");
                }
            });
        }
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
        public bool _IsReplaceKeyWord = false;
        public bool IsReplaceKeyWord
        {
            get { return _IsReplaceKeyWord; }
            set 
            { 
                SetProperty(ref _IsReplaceKeyWord, value);            
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
        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            DataAccess.Entity<Article>().Load();
            GrabList = DataAccess.Entity<Article>().Local.ToObservableCollection();
            Sort();
        }
        /// <summary>
        /// 对数据进行过滤、排序
        /// </summary>
        public void Sort()
        {
            // 获取 ObservableCollection 的默认视图
            ICollectionView view = CollectionViewSource.GetDefaultView(GrabList);

            // 设置排序规则
            view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));

            // 刷新视图
            view.Refresh();

        }
    }
}

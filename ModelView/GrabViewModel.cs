using BeginSEO.Data;
using BeginSEO.Model;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace BeginSEO.ModelView
{
    public class GrabViewModel : ObservableObject
    {
        public GrabViewModel() 
        {
            GrabCommand = new RelayCommand(GrabHandle);
            RemoveAllCommand = new RelayCommand(RemoveAllHandle);
            SelectCommand = new RelayCommand<Article>(SelectHandle);
            BackCommand = new RelayCommand(() => Page = 0);
            OriginalCommand = new RelayCommand(OriginalHandle);
            CopyCommand = new RelayCommand(() =>
            {
                Clipboard.SetText(InArticle.Rewrite);
                ShowToast.Show("已复制成功.");
            });
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
        public ICommand OriginalCommand { get; set; }
        public void OriginalHandle()
        {
            var model = new Model.ReplaceKeyWord();
            var data = DataAccess.Entity<Article>()
                    .Where(I => I.IsUseRewrite == false || I.IsUseReplaceKeyword == false)
                    .ToList();
            Parallel.ForEach(data, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async i =>
            {
                var (ContrastValue, OriginalValue, (O_msg, O_Status), (R_msg, R_Status)) = await model.Original(i.Content, "3", true, IsReplaceKeyWord);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var context = DataAccess.GetDbContext();
                    var find = context.Set<Article>().FirstOrDefault(I => I.Url == i.Url);
                    if (find != null)
                    {
                        find.IsUseReplaceKeyword = IsReplaceKeyWord;
                        find.Rewrite = OriginalValue;
                        find.Contrast = ContrastValue;
                        find.IsUseRewrite = true;
                        find.Title = model.replice(find.Title).Result;
                        context.SaveChanges();
                    }
                });
            });
        }
        /// <summary>
        /// 选择文章
        /// </summary>
        public ICommand SelectCommand { get; set; }
        public void SelectHandle(Article val)
        {
            InArticle = val;
            Page = 1;
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        public ICommand BackCommand { get; set; }
        public ICommand CopyCommand { get; set; }
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
        public int _Page = 0;
        public int Page
        {
            get { return _Page; }
            set
            {
                SetProperty(ref _Page, value);
            }
        }
        /// <summary>
        /// 当前选中的文章
        /// </summary>
        public Article _InArticle;
        public Article InArticle
        {
            get { return _InArticle; }
            set
            {
                SetProperty(ref _InArticle, value);
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

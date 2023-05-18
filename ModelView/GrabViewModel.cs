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
using System.Runtime.Remoting.Contexts;
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
        readonly dataBank Db;
        public GrabViewModel(dataBank _Db) 
        {
            Db = _Db;
            GrabCommand = new RelayCommand(GrabHandle);
            RemoveAllCommand = new RelayCommand(RemoveAllHandle);
            SelectCommand = new RelayCommand<Article>(SelectHandle);
            BackCommand = new RelayCommand(() => Page = 0);
            OriginalCommand = new AsyncRelayCommand(OriginalHandle);
            ChangeState = new RelayCommand<bool>(ChangeStateHandle);
            InOriginalCommand = new RelayCommand<Article>(InOriginalHandle);
            RemoveDuplicateCommand = new RelayCommand(RemoveDuplicate);
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
            var run = new GrabArticle(Db, _39Grab, GrabLink, GrabCount);
            var result = await run.Grab(IsUseProxy);
            // 
            run.GrabArticles(result, IsUseProxy);
        }
        /// <summary>
        /// 修改文章状态
        /// </summary>
        public ICommand ChangeState { get; set; }
        public void ChangeStateHandle(bool IsCheck)
        {
            InArticle.IsUse = IsCheck;
            Db.SaveChanges();
        }
        /// <summary>
        /// 删除所有文章
        /// </summary>
        public ICommand RemoveAllCommand { get; set; }
        public void RemoveAllHandle()
        {
            ShowModal.Show("提示", "这将删除所有文章，请三四~", async (res) =>
            {
                if (res)
                {
                    var data = GrabList.ToArray();
                    foreach (var item in data)
                    {
                        GrabList.Remove(item);
                    }
                    await Db.SaveChangesAsync();
                    await ShowToast.Show("已删除");
                }
            });
        }
        public ICommand RemoveDuplicateCommand { get; set; }
        public void RemoveDuplicate()
        {
            ShowModal.Show("提示", "这将删除所有重复的文章~", async (res) =>
            {
                if (res)
                {
                    var duplicateUrl = GrabList
                        .GroupBy(p => p.Url)
                        .Where(g => g.Count() > 1)
                        .ToList();

                    foreach (var item in duplicateUrl)
                    {
                        var residue = item.Skip(1);
                        if (residue.Count() > 0)
                        {
                            Db.Set<Article>().RemoveRange(residue);
                        }
                    }
                    Db.SaveChanges();
                }
            });

        }
        /// <summary>
        /// 对文章进行伪原创
        /// </summary>
        public ICommand OriginalCommand { get; set; }
        public async Task OriginalHandle()
        {
            ShowModal.ShowLoading();
            var model = new Model.ReplaceKeyWord();
            var data = GrabList.Where(I => I.IsUseRewrite == false || I.IsUseReplaceKeyword == false || I.Contrast == 0)
                .Take(5);
            foreach (var item in data)
            {
                // 执行网络请求的代码
                string waitStr = string.IsNullOrEmpty(item.Rewrite) ? item.Content : item.Rewrite;
                var (ContrastValue, OriginalValue, (O_msg, O_Status), (R_msg, R_Status)) = await model.Original(waitStr, "3", true, IsReplaceKeyWord);
                string title = await model.replice(item.Title);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    item.IsUseReplaceKeyword = R_Status;
                    item.Rewrite = OriginalValue;
                    item.Contrast = ContrastValue;
                    item.IsUseRewrite = O_Status;
                    item.Title = title;
                    var result = Db.SaveChanges();
                    if (result <= 0)
                    {
                        ShowToast.Error("保存失败~");
                    }
                });
            }
            //int maxConcurrentRequests = 5; // 同时允许的最大请求数量
            //int requestsPerSecond = 5; // 每秒允许的请求数量

            //var semaphore = new SemaphoreSlim(maxConcurrentRequests, maxConcurrentRequests);
            //var tasks = new List<Task>();

            //foreach(var item in data)
            //{
            //    await semaphore.WaitAsync(); // 等待可用的信号量

            //    tasks.Add(Task.Run(async () =>
            //    {
            //        try
            //        {
            //            // 执行网络请求的代码
            //            string waitStr = string.IsNullOrEmpty(item.Rewrite) ? item.Content : item.Rewrite;
            //            var (ContrastValue, OriginalValue, (O_msg, O_Status), (R_msg, R_Status)) = await model.Original(waitStr, "3", true, IsReplaceKeyWord);
            //            string title = await model.replice(item.Title);
            //            Application.Current.Dispatcher.Invoke(()=>
            //            {
            //                item.IsUseReplaceKeyword = R_Status;
            //                item.Rewrite = OriginalValue;
            //                item.Contrast = ContrastValue;
            //                item.IsUseRewrite = O_Status;
            //                item.Title = title;
            //                var result = Db.SaveChanges();
            //                if (result <= 0)
            //                {
            //                    ShowToast.Error("保存失败~");
            //                }
            //            });
            //        }
            //        finally
            //        {
            //            semaphore.Release(); // 释放信号量
            //        }
            //    }));

            //    if (tasks.Count >= requestsPerSecond)
            //    {
            //        await Task.WhenAny(tasks); // 等待每秒请求限制
            //        tasks.RemoveAll(t => t.IsCompleted);
            //        await Task.Delay(1000 / requestsPerSecond); // 等待1秒
            //    }
            //}

            //await Task.WhenAll(tasks); // 等待所有请求完成
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    ShowModal.Closing();
            //});
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
        /// <summary>
        /// 对选定的文章进行伪原创
        /// </summary>
        public ICommand InOriginalCommand { get; set; }
        public void InOriginalHandle(Article item)
        {
            ShowModal.ShowLoading();
            Task.Run(async () =>
            {
                var model = new Model.ReplaceKeyWord();
                var (ContrastValue, OriginalValue, (O_msg, O_Status), (R_msg, R_Status)) = await model.Original(item.Rewrite, "3", item.IsUseRewrite, item.IsUseReplaceKeyword);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //var data = GrabList.FirstOrDefault(I=>I.Url == InArticle.Url);
                    item.Rewrite = OriginalValue;
                    item.Contrast = ContrastValue;
                    Db.SaveChanges();
                    if (item.IsUseRewrite)
                    {
                        ShowToast.Show(O_msg, O_Status ? ShowToast.Type.Success : ShowToast.Type.Warning);
                    }
                    if (item.IsUseReplaceKeyword)
                    {
                        ShowToast.Show(R_msg, O_Status ? ShowToast.Type.Success : ShowToast.Type.Warning);
                    }
                    ShowModal.Closing();
                });
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
            Db.Set<Article>()
                .Where(I=> !I.IsUse)
                .Load();
            GrabList = Db.Set<Article>().Local.ToObservableCollection();
        }
    }
}

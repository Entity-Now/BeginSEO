using BeginSEO.Data;
using BeginSEO.Model;
using BeginSEO.SQL;
using BeginSEO.Utils;
using BeginSEO.Utils._5118;
using BeginSEO.Utils.Dependency;
using BeginSEO.Utils.Exceptions;
using BeginSEO.Utils.Spider;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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
        public readonly _5118Dependency _5118s;
        public GrabViewModel(dataBank _Db, _5118Dependency _5118s)
        {
            Db = _Db;
            this._5118s = _5118s;
            GrabCommand = new RelayCommand(GrabHandle);
            RemoveAllCommand = new RelayCommand(RemoveAllHandle);
            SelectCommand = new RelayCommand<Article>(SelectHandle);
            BackCommand = new RelayCommand(() => Page = 0);
            OriginalCommand = new AsyncRelayCommand(OriginalHandle);
            ChangeState = new RelayCommand<bool>(ChangeStateHandle);
            InOriginalCommand = new AsyncRelayCommand<Article>(InOriginalHandle);
            RemoveDuplicateCommand = new RelayCommand(RemoveDuplicate);
            CopyCommand = new RelayCommand(() =>
            {
                Clipboard.SetText(InArticle.Rewrite);
                ShowToast.Show("已复制成功.");
            });
            RefreshCommand = new RelayCommand(() => LoadData());
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
            IQueryable<Article> data;
            if (OriginalAll)
            {
                data = Db.Set<Article>()
                    .Where(I => !I.IsUse);
            }
            else
            {
                data = Db.Set<Article>()
                    .Where(I => !Unqualified ? I.Contrast <= 0 : (I.Contrast > 70 || I.Contrast <= 0))
                    .Where(I => !I.IsUse);
            }
            var ks = Db.Set<KeyWord>().ToList();
            var _5118 = new ReplaceKeyWordTools(ks, _5118s.ROriginal, _5118s.RAkey);

            await Tools.ExecuteTaskHandle<Article>(data, 5, async (item) =>
            {
                var result = await _5118.Original(item.Content, "3", IsRewrite, IsReplaceKeyWord);
                item.Title = _5118.replaceKeyWord(item.Title);
                await UpdateArticle(item, result);
            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                ShowModal.Closing();
            });
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task UpdateArticle(Article item, OriginalResult result)
        {
            try
            {
                //var find = await Db.Set<Article>().FirstAsync(i=> i.Id == item.Id);
                item.Rewrite = result.NewValue;
                item.Contrast = result.contrastValue;
                item.IsUseRewrite = result.OriginalStatus;
                item.IsUseReplaceKeyword = result.AkeyStatus;
                await Db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new LoggingException($"UpdateArticle {e.Message}");
            }
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
        public ICommand RefreshCommand { get; set; }
        public async Task InOriginalHandle(Article item)
        {
            ShowModal.ShowLoading();
            var ks = await Db.Set<KeyWord>().ToListAsync();
            var _5118 = new ReplaceKeyWordTools(ks, _5118s.ROriginal, _5118s.RAkey);
            var result = await _5118.Original(item.Rewrite, "3", IsRewrite, IsReplaceKeyWord);
            await UpdateArticle(item, result);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (result.OriginalStatus)
                {
                    ShowToast.Success(result.OriginalError);
                }
                else
                {
                    ShowToast.Error(result.OriginalError);
                }
                if (result.AkeyStatus)
                {
                    ShowToast.Success(result.AkeyError);
                }
                else
                {
                    ShowToast.Error(result.AkeyError);
                }
                ShowModal.Closing();
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
        public bool _IsReplaceKeyWord = true;
        public bool IsReplaceKeyWord
        {
            get { return _IsReplaceKeyWord; }
            set 
            { 
                SetProperty(ref _IsReplaceKeyWord, value);            
            }
        }
        public bool _IsRewrite = true;
        public bool IsRewrite
        {
            get => _IsRewrite;
            set
            {
                SetProperty(ref _IsRewrite, value);
            }
        }
        public bool _OriginalAll = false;
        public bool OriginalAll
        {
            get=> _OriginalAll;
            set
            {
                SetProperty(ref _OriginalAll, value);
            }
        }
        public bool _IsUseProxy = false;
        public bool IsUseProxy
        {
            get => _IsUseProxy;
            set
            {
                _IsUseProxy = value;
                SetProperty(ref _IsUseProxy, value);
            }
        }
        /// <summary>
        /// 相似度超标的，不合格
        /// </summary>
        public bool _Unqualified = false;
        public bool Unqualified
        {
            get => _Unqualified;
            set
            {
                SetProperty(ref _Unqualified, value);
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

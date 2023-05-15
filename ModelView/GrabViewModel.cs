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
            ChangeState = new RelayCommand<bool>(ChangeStateHandle);
            InOriginalCommand = new RelayCommand<Article>(InOriginalHandle);
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
        /// <summary>
        /// 修改文章状态
        /// </summary>
        public ICommand ChangeState { get; set; }
        public void ChangeStateHandle(bool IsCheck)
        {
            InArticle.IsUse = IsCheck;
            DataAccess.SaveChanges();
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
                    await DataAccess.BeginContext.SaveChangesAsync();
                    await ShowToast.Show("已删除");
                }
            });
        }
        /// <summary>
        /// 对文章进行伪原创
        /// </summary>
        public ICommand OriginalCommand { get; set; }
        public void OriginalHandle()
        {
            ShowModal.ShowLoading();
            var model = new Model.ReplaceKeyWord();
            var data = GrabList.Where(I => I.IsUseRewrite == false || I.IsUseReplaceKeyword == false || I.Contrast == 0).ToList();
            int Aggregate = data.Count;
            Parallel.ForEach(data, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async i =>
            {
                try
                {
                    var (ContrastValue, OriginalValue, (O_msg, O_Status), (R_msg, R_Status)) = await model.Original(i.Content, "3", true, IsReplaceKeyWord);
                    var find = GrabList.FirstOrDefault(I => I.Url == i.Url);
                    if (find != null)
                    {
                        find.IsUseReplaceKeyword = IsReplaceKeyWord;
                        find.Rewrite = OriginalValue;
                        find.Contrast = ContrastValue;
                        find.IsUseRewrite = true;
                        find.Title = await model.replice(find.Title);
                    }
                    Interlocked.Decrement(ref Aggregate);
                    if (Aggregate % 10 == 0)
                    {
                        await DataAccess.BeginContext.SaveChangesAsync();
                    }
                    if (Aggregate <= 0)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            DataAccess.SaveChanges();
                            ShowModal.Closing();
                        });
                    }
                }
                catch (Exception e)
                {
                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        ShowToast.Show(e.Message, ShowToast.Type.Info);
                    });
                }

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
                    DataAccess.SaveChanges();
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
            DataAccess.Entity<Article>().Load();
            GrabList = DataAccess.Entity<Article>().Local.ToObservableCollection();
        }
    }
}

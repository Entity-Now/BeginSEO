using BeginSEO.Components;
using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.Model;
using BeginSEO.Services;
using BeginSEO.SQL;
using BeginSEO.Utils;
using BeginSEO.Utils._5118;
using BeginSEO.Utils.Dependency;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard;

namespace BeginSEO.ModelView
{
    public class KeyWordReplaceViewModel : ObservableObject
    {
        public readonly dataBank Db;
        public readonly _5118Service _5118s;
        public KeyWordReplaceViewModel(dataBank db, _5118Service _5118s)
        {
            Db = db;
            this._5118s = _5118s;
            AddKeyWord = new RelayCommand(AddKeyWordHandle);
            RemoveKeyWord = new RelayCommand<KeyWord>(RemoveKeyWordHandle);
            SelectionKeyWord = new RelayCommand<KeyWord>(SelectionKeyWordHandle);
            SelectionStrict = new RelayCommand<ComboBoxItem>(SelectionStrictHandle);
            Clear = new RelayCommand(ClearHandle);
            Operate = new RelayCommand(OperateHandle);
            Load();
        }
        public void Load()
        {
            Db.Set<KeyWord>().Load();
            KeyWords = Db.Set<KeyWord>().Local.ToObservableCollection();
        }
        public string _KeyWord;
        public string KeyWord
        {
            get { return _KeyWord; }
            set
            {
                SetProperty(ref _KeyWord, value);
            }
        }
        public string _KeyWordList;
        public string KeyWordList
        {
            get { return _KeyWordList; }
            set
            {
                SetProperty(ref _KeyWordList, value);
            }
        }
        /// <summary>
        /// 优先度
        /// </summary>
        public int _Level = 1;
        public int Level
        {
            get { return _Level; }
            set { SetProperty(ref _Level, value);}
        }
        /// <summary>
        /// 保护词
        /// </summary>
        public bool _IsLockKeyWord;
        public bool IsLockKeyWord
        {
            get
            {
                return _IsLockKeyWord;
            }
            set
            {
                SetProperty(ref _IsLockKeyWord, value);
            }
        }
        /// <summary>
        /// 原文
        /// </summary>
        public string _TheOriginal;
        public string TheOriginal
        {
            get=> _TheOriginal;
            set
            {
                SetProperty(ref _TheOriginal, value);
            }
        }
        /// <summary>
        /// 伪原创后的内容
        /// </summary>
        public string _Original;
        public string Original
        {
            get => _Original;
            set
            {
                SetProperty(ref _Original, value);
            }
        }
        /// <summary>
        /// 换词严格度
        /// </summary>
        public string _Strict;
        public string Strict
        {
            get => _Strict;
            set
            {
                SetProperty(ref _Strict, value);
            }
        }
        public ObservableCollection<Tuple<string, bool>> _Freatures = new ObservableCollection<Tuple<string, bool>> 
        {
            new Tuple<string, bool>("MarkToHtml", false),
            new Tuple<string, bool>("智能改写", false),
            new Tuple<string, bool>("智能改写（新）", false),
            new Tuple<string, bool>("一键换词", false),
            new Tuple<string, bool>("一键换词", false),
            new Tuple<string, bool>("复制到剪切板", true),
        };
        public ObservableCollection<Tuple<string, bool>> Freatures
        {
            get => _Freatures;
            set
            {
                SetProperty(ref _Freatures, value);
            }
        }
        /// <summary>
        /// 是否将Markdown代码转换为HTML代码
        /// </summary>
        public bool _IsConvertMark = false;
        public bool IsConvertMark
        {
            get => _IsConvertMark;
            set
            {
                SetProperty(ref _IsConvertMark, value);
            }
        }

        public ObservableCollection<KeyWord> _KeyWords;
        public ObservableCollection<KeyWord> KeyWords
        {
            get => _KeyWords;
            set
            {
                SetProperty(ref _KeyWords, value);
            }
        }

        /// <summary>
        /// 添加关键词
        /// </summary>
        public ICommand AddKeyWord { get; set; }
        public void AddKeyWordHandle()
        {
            if (string.IsNullOrEmpty(KeyWord) || string.IsNullOrEmpty(KeyWordList))
            {
                ShowToast.Show("请输入关键词...", ShowToast.Type.Warning);
                return;
            }
            var find = KeyWords.FirstOrDefault(I => I.Key == KeyWord);
            if (find != null)
            {
                find.Value = KeyWordList;
                find.level = Level;
                find.Type = IsLockKeyWord;
            }
            else
            {
                KeyWords.Add(new Data.KeyWord
                {
                    Key = KeyWord,
                    Value = KeyWordList,
                    level = Level,
                    Type = IsLockKeyWord
                });
            }
            Db.SaveChanges();
            Refresh();
        }
        /// <summary>
        /// 删除关键词
        /// </summary>
        public ICommand RemoveKeyWord { get; private set; }
        public void RemoveKeyWordHandle(KeyWord item)
        {
            var find = KeyWords.FirstOrDefault(I => I == item);
            if (find != null)
            {
                KeyWords.Remove(find);
                Db.SaveChanges();
            }
            // refresh
            Refresh();
        }
        /// <summary>
        /// 选择关键词
        /// </summary>
        public ICommand SelectionKeyWord { get; private set; }
        public void SelectionKeyWordHandle(KeyWord item)
        {
            Refresh(item.Key, item.Value, item.level, item.Type);
        }
        void Refresh(string _keyword = "", string _KeyWordList = "", int _Level = 1, bool _IsLockKeyWord = false)
        {
            KeyWord = _keyword;
            KeyWordList = _KeyWordList;
            Level = _Level;
            IsLockKeyWord = _IsLockKeyWord;
        }
        /// <summary>
        /// 选择换词严格度
        /// </summary>
        public ICommand SelectionStrict { get; private set; }
        public void SelectionStrictHandle(ComboBoxItem item)
        {
            Strict = item.Content.ToString();
        }
        /// <summary>
        /// 删除文本内容
        /// </summary>
        public ICommand Clear {  get; private set; }
        public void ClearHandle()
        {
            Original = string.Empty;
            TheOriginal = string.Empty;
        }
        /// <summary>
        /// 一键操作
        /// </summary>
        public ICommand Operate { get; private set; }
        public async void OperateHandle()
        {
            ShowModal.ShowLoading();
            if (string.IsNullOrEmpty(TheOriginal))
            {
                // 获取剪切板的数据
                TheOriginal = Clipboard.GetText();
            }

            var _5118 = new ReplaceKeyWordTools(KeyWords.ToList(), _5118s.ROriginal, _5118s.RAkey, _5118s.RNewOriginal);
            var result = await _5118.Original(TheOriginal, Strict, IsOriginal, IsReplace, IsNewOriginal);
            Similars = result.contrastValue;
            Original = result.NewValue;
            // 判断是否原创成功
            if (IsOriginal)
            {
                await ShowToast.Show(result.OriginalError);
            }
            if (IsReplace)
            {
                await ShowToast.Show(result.AkeyError);
            }
            if (IsConvertMark)
            {
                Original = MarkdownHelper.MarkToHtml(Original);
            }
            if (IsCopy)
            {
                Clipboard.SetText(Original);
            }
            ShowModal.Closing();
        }
    }
}

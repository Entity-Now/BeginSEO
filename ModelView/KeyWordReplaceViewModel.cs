﻿using BeginSEO.Data;
using BeginSEO.Model;
using BeginSEO.SQL;
using BeginSEO.Utils;
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
using System.Windows.Input;

namespace BeginSEO.ModelView
{
    public class KeyWordReplaceViewModel : ObservableObject
    {
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
        /// 是否要5118替换关键词
        /// </summary>
        public bool _IsReplace;
        public bool IsReplace
        {
            get => _IsReplace;
            set
            {
                SetProperty(ref _IsReplace, value);
            }
        }
        /// <summary>
        /// 是否要伪原创
        /// </summary>
        public bool _IsOriginal;
        public bool IsOriginal
        {
            get => _IsOriginal;
            set
            {
                SetProperty(ref _IsOriginal, value);
            }
        }
        /// <summary>
        /// 原创度
        /// </summary>
        public double _Similars;
        public double Similars
        {
            get => _Similars;
            set
            {
                SetProperty(ref _Similars, value);
            }
        }
        public bool _IsCopy = true;
        public bool IsCopy
        {
            get=>_IsCopy;
            set
            {
                SetProperty(ref _IsCopy, value);
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

        public ObservableCollection<KeyWord> _KeyWords;
        public ObservableCollection<KeyWord> KeyWords
        {
            get => _KeyWords;
            set
            {
                SetProperty(ref _KeyWords, value);
            }
        }

        public Model.ReplaceKeyWord Model = new Model.ReplaceKeyWord();
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
            DataAccess.SaveChanges();
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
                DataAccess.SaveChanges();
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
            var (ContrastValue, OriginalValue, (O_msg, O_Status),(R_msg, R_Status)) = await Model.Original(TheOriginal, Strict, IsOriginal, IsReplace);
            Similars = ContrastValue;
            Original = OriginalValue;
            // 判断是否原创成功
            if (IsOriginal)
            {
                await ShowToast.Show(O_msg);
            }
            if (IsReplace)
            {
                await ShowToast.Show(R_msg);
            }
            if (IsCopy)
            {
                Clipboard.SetText(OriginalValue);
            }
            ShowModal.Closing();
        }

        public KeyWordReplaceViewModel()
        {
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
            DataAccess.Entity<KeyWord>().Load();
            KeyWords = DataAccess.Entity<KeyWord>().Local.ToObservableCollection();
        }
    }
}

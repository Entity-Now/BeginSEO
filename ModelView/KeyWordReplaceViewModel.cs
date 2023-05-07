using BeginSEO.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeginSEO.ModelView
{
    internal class KeyWordReplaceViewModel : ObservableObject
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
        public int _Level;
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
        public string _IsReplace;
        public string IsReplace
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
        public string _IsOriginal;
        public string IsOriginal
        {
            get => _IsOriginal;
            set
            {
                SetProperty(ref _IsOriginal, value);
            }
        }

        public ReplaceKeyWord Model = new ReplaceKeyWord();
        /// <summary>
        /// 添加关键词
        /// </summary>
        public ICommand AddKeyWord { get; set; }
        public void AddKeyWordHandle()
        {
            
        }
        /// <summary>
        /// 删除关键词
        /// </summary>
        public ICommand RemoveKeyWord { get; private set; }
        public void RemoveKeyWordHandle()
        {

        }
        /// <summary>
        /// 删除文本内容
        /// </summary>
        public ICommand Clear {  get; private set; }
        public void ClearHandle()
        {

        }
        /// <summary>
        /// 一键操作
        /// </summary>
        public ICommand Operate { get; private set; }
        public void OperateHandle()
        {

        }
    }
}

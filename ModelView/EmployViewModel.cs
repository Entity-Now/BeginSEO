﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeginSEO.Model;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using BeginSEO.Utils;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Microsoft.Win32;
using BeginSEO.Data;
using System.IO.Compression;
using System.IO;
using BrotliSharpLib;
using System.Windows.Threading;
using System.Windows.Controls;
using BeginSEO.SQL;
using System.Threading;

namespace BeginSEO.ModelView
{
    public class EmployViewModel: ObservableObject
    {
        public EmployViewModel()
        {
            Handle = new RelayCommand(GetEmploy);
            ClearList = new RelayCommand(Clear);
            CommandShowEmploy = new RelayCommand(ShowEmploy);
            CommandRemove = new RelayCommand<EmployData>(Remove);
            CommandCopy = new RelayCommand<EmployData>(Copy);
            ReHandleCommand = new RelayCommand(ReHandle);
        }
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
        public List<string> ErrorList { get; set; } = null;
        public ICommand ReHandleCommand { get; set; }
        public ICommand Handle { get; set; }
        public ICommand ClearList { get; set; }
        public void Clear()
        {
            // 清空列表
            EmployList.Clear();
        }
        /// <summary>
        /// 查询收录的链接
        /// </summary>
        void GetEmploy() {
            // 清空列表
            Clear();
            if (string.IsNullOrEmpty(UrlList))
            {
                return;
            }
            // 分割地址
            List<string> urlList = UrlList.Split('\r').ToList();
            Employ(urlList);
        }
        void Employ(List<string> urlList)
        {
            new Thread(async () => {
                ErrorList = await HTTP.MultipleRequest(urlList, new Progress<EmployData>(I =>
                {
                    Tools.Dispatcher(() =>
                    {
                        EmployList.Add(I);
                    });
                }));
            }).Start();
        }
        /// <summary>
        /// 重新查询失败的链接
        /// </summary>
        void ReHandle()
        {
            if (ErrorList == null)
            {
                ShowToast.Show("没有查询失败的链接");
            }
            else
            {
                Employ(ErrorList);
            }
        }
        public ICommand CommandRemove { get; set; }
        public void Remove(EmployData listViewItem)
        {
            EmployList.Remove(listViewItem);
            ShowToast.Open("删除成功");
        }
        public ICommand CommandCopy { get; set; }
        public void Copy(EmployData Item)
        {
            Clipboard.SetText(Item.Url.Trim());
            ShowToast.Open("复制成功");
        }
        /// <summary>
        /// 只显示已收录的链接
        /// </summary>
        public ICommand CommandShowEmploy { get; set; }
        public void ShowEmploy()
        {
            var data = EmployList
                .Where(I => I.Status == "未收录" || I.Status == "请求失败" || I.Status == "需要验证")
                .ToList();
            foreach (var item in data)
            {
                EmployList.Remove(item);
            }

        }
    }
}

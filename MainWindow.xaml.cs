﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeginSEO.Attributes;
using BeginSEO.Components;
using BeginSEO.Model;
using BeginSEO.ModelView;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace BeginSEO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private Dictionary<string, object> PageList = new Dictionary<string, object>();
        public MainWindow() {
            InitializeComponent();
            // 注入提示框
            Inject();
            // 获取所有窗口
            var data = getReflex.Get<PagesAttribute>("BeginSEO")
                .OrderBy(I=> I.GetCustomAttribute<PagesAttribute>().Orderby)
                .ThenByDescending(I=> I.GetCustomAttribute<PagesAttribute>().Name.Length);
            foreach (var page in data)
            {
                var PageInfo = page.GetCustomAttribute<PagesAttribute>();
                var rb = new RadioButton
                {
                    Content = PageInfo.Name,
                    Height = 32,
                    Margin = new Thickness(1),
                    Style = (Style)FindResource("m_Radio"),
                    IsChecked = PageInfo.IsHome
                };
                object temp_page = null;
                if (PageList.ContainsKey(PageInfo.Name))
                {
                    temp_page = PageList[PageInfo.Name];
                }
                else
                {
                    temp_page = Activator.CreateInstance(page);
                    if (temp_page is UserControl home)
                    { 
                        Type Page_Type = (Type)temp_page.GetType().GetField("MyType")?.GetValue(null);
                        if (Page_Type != null)
                        {
                            var GetRequiredService = typeof(IServiceProvider).GetMethod("GetService")
                                .Invoke(App.Current.Services, new object[] { Page_Type });
                            home.DataContext = GetRequiredService;
                        }
                        PageList.Add(PageInfo.Name, home);
                    }
                }
                rb.Click += (sender, e) =>
                {
                    MainContent.Content = temp_page;
                };
                NavList.Children.Add(rb);
                if (PageInfo.IsHome)
                {
                    MainContent.Content = temp_page;
                }
            }
        }

        void Inject()
        {
            ShowToast.Snackbar = MainSnackbar;
            ShowModal.dialogHost = MainDialog;
        }
    }
}

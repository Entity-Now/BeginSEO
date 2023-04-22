﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeginSEO.Attributes;
using BeginSEO.Model;
using BeginSEO.ModelView;

namespace BeginSEO.Components
{
    /// <summary>
    /// Employ.xaml 的交互逻辑
    /// </summary>
    [Pages("百度收录查询")]
    public partial class Employ : UserControl
    {
        public EmployViewModel ViewModel { get; set; }
        public Employ()
        {
            InitializeComponent();
            ViewModel = new EmployViewModel();
            this.DataContext = ViewModel;
        }
    }
}
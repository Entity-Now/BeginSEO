﻿using BeginSEO.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BeginSEO {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        protected override void OnExit(ExitEventArgs e)
        {
            // 释放日志
            Logging.CloseLogger();
            base.OnExit(e);
        }
    }
}

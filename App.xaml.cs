using BeginSEO.ModelView;
using BeginSEO.SQL;
using BeginSEO.Utils;
using BeginSEO.Utils.Spider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services = ConfigureServices();
            ServiceLocator.Initialize(Services);
        }
        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            // 注册DbContext
            services.AddDbContext<dataBank>();
            // 注册其他依赖项
            services.AddTransient<MainWindow>();
            services.AddTransient<GrabViewModel>();
            services.AddTransient<GrabArticle>();
            services.AddTransient<KeyWordReplaceViewModel>();
            services.AddTransient<EmployViewModel>();
            services.AddTransient<SettingsViewModel>();

            return services.BuildServiceProvider();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 释放日志
            Logging.CloseLogger();
            base.OnExit(e);
        }
    }
}

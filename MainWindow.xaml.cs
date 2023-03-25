using System;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeginSEO.Components;
using BeginSEO.Model;
using BeginSEO.ModelView;
using BeginSEO.SQL;
using BeginSEO.Utils;

namespace BeginSEO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainModelView ViewModel { get; set; }
        public MainWindow() {
            InitializeComponent();
            ViewModel = new MainModelView();
            this.DataContext = ViewModel;
            // 注入提示框
            Inject();
            // 初始化数据库
            DataAccess.init();
        }

        void Inject()
        {
            ShowToast.Snackbar = MainSnackbar;
            ShowModal.dialogHost = MainDialog;
        }
    }
}

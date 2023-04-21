using BeginSEO.Attributes;
using BeginSEO.ModelView;
using BeginSEO.SQL;
using System;
using System.Collections.Generic;
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

namespace BeginSEO.View
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    [Pages("设置")]
    public partial class Settings : UserControl
    {
        SettingsViewModel viewModel;
        public Settings()
        {
            InitializeComponent();
            viewModel = new SettingsViewModel();
            this.DataContext = viewModel;
        }
    }
}

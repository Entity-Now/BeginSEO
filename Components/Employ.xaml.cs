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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 替换关键词.Model;
using 替换关键词.ModelView;

namespace 替换关键词.Components
{
    /// <summary>
    /// Employ.xaml 的交互逻辑
    /// </summary>
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

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
using 替换关键词.Model;

namespace 替换关键词 {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            // init
            JsonUtils.Init();
            KeyWrodList.ItemsSource = JsonUtils.Ks;

        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_Delete_Click(object sender, RoutedEventArgs e) {
            var item = (sender as Button).Tag as string;
            if (item != null) {
                MessageBox.Show(item);
            }
        }
    }
}

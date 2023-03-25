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

namespace BeginSEO.Components
{
    /// <summary>
    /// MessageModal.xaml 的交互逻辑
    /// </summary>
    public partial class MessageModal : UserControl
    {
        public Action<bool> CallBack { get; set; }

        public MessageModal(string title, string value)
        {
            InitializeComponent();
            xTitle.Text = title;
            xValue.Text = value;
            this.Cancel.Visibility = Visibility.Hidden;
        }
        public MessageModal(string title, string value, Action<bool> callBack = null):this(title, value)
        {
            this.Cancel.Visibility = Visibility.Visible;
            this.CallBack = callBack;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CallBack(false);
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            CallBack(true);
        }
    }
}

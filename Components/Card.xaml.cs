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
    /// Card.xaml 的交互逻辑
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }
        public object Title { get; set; }
        public object Description { get; set; }
        public object Button { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _Description.Content = Description;
            _Title.Content = Title;
            _Button.Content = Button;
        }
    }
}

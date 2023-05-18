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
using BeginSEO.Attributes;
using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.Model;
using BeginSEO.ModelView;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeginSEO.Components
{
    /// <summary>
    /// KeyWordReplice.xaml 的交互逻辑
    /// </summary>
    [Pages("关键词替换", IsHome:true)]
    public partial class KeyWordReplice : UserControl
    {
        public static Type MyType = typeof(KeyWordReplaceViewModel);
        public KeyWordReplice()
        {
            InitializeComponent();
        }
    }
}

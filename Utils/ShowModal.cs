using BeginSEO.Components;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeginSEO.Utils
{
    public static class ShowModal
    {
        public static DialogHost dialogHost {  get; set; }
        public static void Show(string title, string value, Action<bool> callBack = null)
        {
            dialogHost.ShowDialog(new MessageModal(title, value, callBack));
        }
    }
}

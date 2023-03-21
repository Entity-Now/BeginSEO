using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BeginSEO.Utils
{
    public static class ShowToast
    {
        public static Snackbar Snackbar { get; set; }

        public static void Open(string Content)
        {
            //Error();
            Snackbar.MessageQueue.Enqueue(Content);
        }

        public static void Error()
        {
            Snackbar.Background = new SolidColorBrush(Colors.Red);
        }
    }
}

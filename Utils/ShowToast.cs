using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BeginSEO.Utils
{
    public static class ShowToast
    {
        public static Snackbar Snackbar { get; set; }
        public enum Type 
        { 
            Info,
            Warning,
            Success,
            Error
        }

        public static void Open(string Content)
        {
            Snackbar.MessageQueue.Enqueue(Content);
        }

        public static async Task Show(string Content, Type type = Type.Info)
        {
            while (Snackbar.MessageQueue.QueuedMessages.Count > 0)
            {
                await Task.Delay(1000);
            }
            Snackbar.Background = GetColor(type);
            Open(Content);
        }
        static Brush GetColor(Type type)
        {
            switch (type)
            {
                case Type.Info:
                    return Tools.GetBrush("InfoBrush");
                case Type.Success:
                    return Tools.GetBrush("SuccessBrush");
                case Type.Warning:
                    return Tools.GetBrush("WarningBrush");
                case Type.Error:
                    return Tools.GetBrush("ErrorBrush");
                default:
                    return Tools.GetBrush("ErrorBrush");
            }
        }
    }
}

using BeginSEO.Data.DataEnum;
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
            // 通过反射输出日志
            getReflex.InvokeStaticMethod(typeof(Logging), type.ToString(), Content);
        }
        public static async Task Info(string Content) => await Show(Content, Type.Info);
        public static async Task Warning(string Content) => await Show(Content, Type.Warning);
        public static async Task Success(string Content) => await Show(Content, Type.Success);
        public static async Task Error(string Content) => await Show(Content, Type.Error);

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

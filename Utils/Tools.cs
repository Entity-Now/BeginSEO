using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BeginSEO.Utils
{
    public static class Tools
    {
        public static object GetResource(string DictionaryName)
        {
            // 获取指定资源字典中的资源
            return Application.Current.Resources[DictionaryName];
        }
        public static Brush GetBrush(string Name)
        {
            return GetResource(Name) as SolidColorBrush;
            //return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }
        public static string GetTimeStamp()
        {
            return $"{(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000}";
        }
    }
}

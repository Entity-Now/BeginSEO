using BeginSEO.Attributes;
using BeginSEO.Data;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeginSEO.Utils
{
    public static class getReflex
    {
        /// <summary>
        /// 使用反射获取所有包含database特性的类
        /// </summary>
        /// <returns></returns>
        public static List<TypeInfo> Get<T>(string AssemblyName) where T : Attribute
        {
           // 反射获取数据库
           var refs = AppDomain.CurrentDomain.GetAssemblies()
                .First(I => I.GetName().Name == AssemblyName)
                .DefinedTypes
                .Where(I=>I.IsClass)
                .Where(I=>I.GetCustomAttributes<T>().Any())
                .ToList();

            return refs;
        }
        /// <summary>
        /// 通过反射获取包含指定特性的类，并动态调用泛型函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="Method"></param>
        public static void MakeMethod<T>(T source, string Method)
        {
            // 获取指定的方法
            var makeMethod = typeof(T).GetMethod(Method, new Type[] { });
            // 反射获取所有包含指定特性的类
            var DbList = getReflex.Get<databaseAttribute>("BeginSEO");
            foreach (var item in DbList)
            {
                // 给获取到的函数赋值，并传递泛型的类型
                makeMethod.MakeGenericMethod(item)
                    // 调用函数，并传递参数
                    .Invoke(source, new object[] { });
            }
        }
        /// <summary>
        /// 获取对象的propertys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PropertyInfo[] mGetPropertys<T>(this T source) where T : class
        {
            if (source == null)
            {
                return null;
            }
            return source.GetType().GetProperties();
        }
        /// <summary>
        /// 获取对象的property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo mGetProperty<T>(this T source, string name) where T : class
        {
            if (source == null)
            {
                return null;
            }
            return source.GetType().GetProperty(name);
        }
        /// <summary>
        /// 给对象的属性赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void mSetPropertyValue<T>(this T source, string name, object value) where T : class
        {
            if (source == null)
            {
                return;
            }
            source.mGetProperty<T>(name).SetValue(source, value);
        }
        /// <summary>
        /// 对旧的对象赋值
        /// </summary>
        public static void Assignment<old, New>(this old source, New Value) where old : class where New : class
        {
            var oldPropertys = source.mGetPropertys<old>();
            var newPropertys = Value.mGetPropertys<New>();
            foreach (var item in oldPropertys)
            {
                foreach (var item2 in newPropertys)
                {
                    if (item.Name == item2.Name && item2.GetValue(Value,null) != null)
                    {
                        item.SetValue(source, item2);
                    }
                }
            }
        }
    }
}

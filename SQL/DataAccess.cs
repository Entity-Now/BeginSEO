using BeginSEO.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BeginSEO.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;

namespace BeginSEO.SQL
{
    public static class DataAccess
    {
        public static dataBank BeginContext { get; set; }
        public static void init()
        {
            if (BeginContext == null || BeginContext == default)
            {
                BeginContext = new dataBank();
            }
            // 初始化
            var Ensure = BeginContext.Database.EnsureCreated();
            if (Ensure)
            {
                if (File.Exists("KeyWordLists.json"))
                {
                    var json = File.ReadAllText("KeyWordLists.json");
                    var data = JsonConvert.DeserializeObject<List<KeyWord>>(json);
                    foreach (var item in data)
                    {
                        Inser<KeyWord>(item);
                    }
                }
            }
        }
        public static DbSet<T> Entity<T>() where T : class
        {
            return BeginContext.Set<T>();
        }
        public static void SaveChanges()
        {
            BeginContext.SaveChanges();
        }
        public static void Inser<T>(T item) where T : class
        {
            BeginContext.Set<T>()
                .Add(item);
            BeginContext.SaveChanges();
        }
        /// <summary>
        /// 使用反射获取所有属性，对比新旧数据进行修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value">新值</param>
        /// <param name="predicate">判断条件</param>
        public static void update<T>(T Value, Expression<Func<T, bool>> predicate = null) where T : class
        {   
            T data = null;
            if (predicate == null)
            {
                data = Entity<T>().FirstOrDefault();
            }
            else
            {
                data = Entity<T>().FirstOrDefault(predicate);
            }
            if (data != null)
            foreach (var item in typeof(T).GetProperties())
            {
                var ps = data.mGetPropertys();
                foreach (var psItem in ps)
                {
                    var ItemAttribute = psItem.GetCustomAttribute<KeyAttribute>();
                    // 不修改键名
                    if (ItemAttribute == null)
                    {
                        if (psItem.Name == item.Name)
                        {
                            var newValue = Value.mGetProperty(item.Name).GetValue(Value, null);
                            if (newValue != null)
                            {
                                psItem.SetValue(item.Name, newValue);
                            }
                        }
                    }
                }
            }
            SaveChanges();
        }
    }
}

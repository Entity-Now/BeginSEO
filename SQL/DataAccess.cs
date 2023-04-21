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
using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Runtime.Remoting.Contexts;

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
            if (BeginContext.Database.GetPendingMigrations().Any())
            {
                try
                {
                    BeginContext.Database.Migrate(); //执行迁移
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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
        /// 如果数据不存在则插入，如果存在则更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void InsertOrUpdate<T>(T item, Expression<Func<T, bool>> predicate = null) where T : class
        {
            var find = predicate == null ? Entity<T>().FirstOrDefault() : Entity<T>().FirstOrDefault(predicate);
            if (find != null)
            {
                update(item, predicate);
            }
            else
            {
                Inser(item);
            }
        }
        /// <summary>
        /// 使用反射获取所有属性，对比新旧数据进行修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value">新值</param>
        /// <param name="predicate">判断条件</param>
        public static void update<T>(T Value, Expression<Func<T, bool>> predicate = null) where T : class
        {
            var data = predicate == null ? Entity<T>().FirstOrDefault() : Entity<T>().FirstOrDefault(predicate);
            if (data != null)
                foreach (var item in data.mGetPropertys())
                {
                    var ItemAttribute = item.GetCustomAttribute<KeyAttribute>();
                    // 不修改键名
                    if (ItemAttribute == null)
                    {
                        var newValue = Value.mGetProperyValue<T, object>(item.Name);
                        if (newValue != null )
                        {
                            item.SetValue(data, newValue);
                        }
                    }
                }
            SaveChanges();
        }
    }
}

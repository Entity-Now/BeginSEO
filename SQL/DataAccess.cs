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
using System.Threading.Tasks;
using System.Threading;

namespace BeginSEO.SQL
{
    public static class DataAccess
    {
        private static readonly object _lock = new object();
        private static readonly ThreadLocal<dataBank> _threadLocalDbContext = new ThreadLocal<dataBank>();

        public static dataBank BeginContext => GetDbContext();
        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <returns></returns>
        public static dataBank GetDbContext()
        {
            if (_threadLocalDbContext.Value == null)
            {
                lock (_lock)
                {
                    if (_threadLocalDbContext.Value == null)
                    {
                        _threadLocalDbContext.Value = new dataBank();
                    }
                }
            }

            return _threadLocalDbContext.Value;
        }
        /// <summary>
        /// 异步获取数据库实例
        /// </summary>
        /// <returns></returns>
        public static async Task<dataBank> GetDbContextAsync()
        {
            if (_threadLocalDbContext.Value == null)
            {
                lock (_lock)
                {
                    if (_threadLocalDbContext.Value == null)
                    {
                        _threadLocalDbContext.Value = new dataBank();
                    }
                }
            }

            return await Task.FromResult(_threadLocalDbContext.Value);
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void Init()
        {
            var context = GetDbContext();
            // 初始化
            var Ensure = context.Database.EnsureCreated();
            if (Ensure)
            {
                SeedData();
            }
            if (context.Database.GetPendingMigrations().Any())
            {
                try
                {
                    context.Database.Migrate(); //执行迁移
                }
                catch (Exception e)
                {
                    LogException(e);
                }
            }
        }

        private static void SeedData()
        {
            if (File.Exists("KeyWordLists.json"))
            {
                var json = File.ReadAllText("KeyWordLists.json");
                var data = JsonConvert.DeserializeObject<List<KeyWord>>(json);
                foreach (var item in data)
                {
                    Insert(item);
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

        public static void Insert<T>(T item) where T : class
        {
            BeginContext.Set<T>().Add(item);
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
                Update(item, predicate);
            }
            else
            {
                Insert(item);
            }
        }
        /// <summary>
        /// 使用反射获取所有属性，对比新旧数据进行修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue">新值</param>
        /// <param name="predicate">判断条件</param>
        public static void Update<T>(T newValue, Expression<Func<T, bool>> predicate = null) where T : class
        {
            var data = predicate == null ? Entity<T>().FirstOrDefault() : Entity<T>().FirstOrDefault(predicate);
            if (data == null)
            {
                return;
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var keyAttribute = property.GetCustomAttribute<KeyAttribute>();
                // 不修改主键
                if (keyAttribute != null)
                {
                    continue;
                }

                var value = property.GetValue(newValue);
                if (value != null)
                {
                    property.SetValue(data, value);
                }
            }

            SaveChanges();
        }
        private static void LogException(Exception e)
        {
            // 记录日志
        }

        public static void Dispose()
        {
            if (_threadLocalDbContext.IsValueCreated)
            {
                _threadLocalDbContext.Value.Dispose();
            }
        }
    }
}

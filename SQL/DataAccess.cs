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
using Microsoft.Extensions.DependencyInjection;

namespace BeginSEO.SQL
{
    public static class DataAccess
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDataBase(this dataBank context)
        {
            // 初始化
            var Ensure = context.Database.EnsureCreated();
            if (context.Database.GetPendingMigrations().Any())
            {
                try
                {
                    context.Database.Migrate(); //执行迁移
                }
                catch (Exception e)
                {
                    Logging.Error(e.Message);
                }
            }
        }

        public static void Insert<T>(this dataBank BeginContext, T item) where T : class
        {
            BeginContext.Set<T>().Add(item);
            BeginContext.SaveChanges();
        }

        /// <summary>
        /// 如果数据不存在则插入，如果存在则更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void InsertOrUpdate<T>(this dataBank BeginContext, T item, Expression<Func<T, bool>> predicate = null) where T : class
        {
            var find = predicate == null ? BeginContext.Set<T>().FirstOrDefault() : BeginContext.Set<T>().FirstOrDefault(predicate);
            if (find != null)
            {
                BeginContext.Update(find, predicate);
            }
            else
            {
                BeginContext.Insert(item);
            }
        }
        /// <summary>
        /// 使用反射获取所有属性，对比新旧数据进行修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue">新值</param>
        /// <param name="predicate">判断条件</param>
        public static void Update<T>(this dataBank BeginContext, T newValue, Expression<Func<T, bool>> predicate = null) where T : class
        {
            var find = predicate == null ? BeginContext.Set<T>().FirstOrDefault() : BeginContext.Set<T>().FirstOrDefault(predicate);
            if (find == null)
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
                    property.SetValue(find, value);
                }
            }

            BeginContext.SaveChanges();
        }
        private static void LogException(Exception e)
        {
            // 记录日志
        }
    }
}

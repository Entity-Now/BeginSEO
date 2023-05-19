using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeginSEO.Data;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System.IO;
using BeginSEO.Attributes;

namespace BeginSEO.SQL
{
    public class dataBank : DbContext
    {
        public dataBank(DbContextOptions<dataBank> option) : base(option) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string path = $"{Environment.CurrentDirectory}/数据库";
            //Directory.CreateDirectory(path);
            //optionsBuilder.UseSqlite($"Data Source={path}/BeginSeo.db");
        }
        //public dataBank() { }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            getReflex.MakeMethod<ModelBuilder>(modelBuilder, "Entity");
        }
    }
}

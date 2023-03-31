﻿// <auto-generated />
using System;
using BeginSEO.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeginSEO.Migrations
{
    [DbContext(typeof(dataBank))]
    partial class dataBankModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("BeginSEO.Data.KeyWord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("KeyWord");
                });

            modelBuilder.Entity("BeginSEO.Data.Proxys", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("IP")
                        .HasColumnType("TEXT");

                    b.Property<string>("Popt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Proxys");
                });

            modelBuilder.Entity("BeginSEO.Data.TempCookie", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CookieKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("CookieValue")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Host")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProxyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TopTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TempCookie");
                });
#pragma warning restore 612, 618
        }
    }
}

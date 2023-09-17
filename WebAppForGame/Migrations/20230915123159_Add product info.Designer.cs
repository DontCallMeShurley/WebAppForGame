﻿// <auto-generated />
using System;
using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace WebAppForGame.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230915123159_Add product info")]
    partial class Addproductinfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebAppForGame.Data.Payments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("PaymentId")
                        .HasColumnType("longtext");

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("longtext");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("WebAppForGame.Data.Products", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<int>("Coins")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebAppForGame.Data.SerialNumbers", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("serial_number")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("SerialNumbers");
                });

            modelBuilder.Entity("WebAppForGame.Data.log_gameover", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long>("score")
                        .HasColumnType("bigint");

                    b.Property<long>("time")
                        .HasColumnType("bigint");

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("log_gameover");
                });

            modelBuilder.Entity("WebAppForGame.Data.userid_mapping", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("mapped_id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("userid_mapping");
                });

            modelBuilder.Entity("WebAppForGame.Data.userlog_in", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<long>("time")
                        .HasColumnType("bigint");

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("userlog_in");
                });

            modelBuilder.Entity("WebAppForGame.Data.Payments", b =>
                {
                    b.HasOne("WebAppForGame.Data.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}

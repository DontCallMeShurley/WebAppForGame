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
    [Migration("20230901200145_add sn table")]
    partial class addsntable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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
#pragma warning restore 612, 618
        }
    }
}

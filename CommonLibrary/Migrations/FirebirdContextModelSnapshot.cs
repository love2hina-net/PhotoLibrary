﻿// <auto-generated />
using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database;

#nullable disable

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Migrations
{
    [DbContext(typeof(FirebirdContext))]
    partial class FirebirdContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 31);

            modelBuilder.Entity("love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities.FileEntryCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Directory")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.Property<int>("IndexHash")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Directory", "Path" }, "IDX_FileEntryCache_Path");

                    b.ToTable("FileEntryCaches");
                });

            modelBuilder.Entity("love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities.FileEntryIndex", b =>
                {
                    b.Property<string>("Directory")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IndexHash")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.HasIndex(new[] { "Directory", "Path" }, "IDX_FileEntryCache_Path");

                    b.ToSqlQuery("SELECT\r\n \"Id\",\r\n \"Directory\",\r\n \"IndexHash\",\r\n \"Path\",\r\n ROW_NUMBER() OVER (ORDER BY \"Path\" ASC) - 1 AS \"Index\" \r\nFROM \"FileEntryCaches\" \r\nORDER BY\r\n \"Path\" ASC");
                });

            modelBuilder.Entity("love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities.ThumbnailCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IndexHash")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastReferenced")
                        .HasColumnType("TIMESTAMP");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.Property<byte[]>("PngData")
                        .HasColumnType("BLOB SUB_TYPE BINARY");

                    b.Property<int?>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "LastReferenced" }, "IDX_ThumbnailCache_Date");

                    b.HasIndex(new[] { "IndexHash", "Path" }, "IDX_ThumbnailCache_Path");

                    b.ToTable("ThumbnailCaches");
                });
#pragma warning restore 612, 618
        }
    }
}

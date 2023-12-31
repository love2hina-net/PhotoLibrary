﻿using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Migrations
{
    public partial class v0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomRootEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "VARCHAR(260)", maxLength: 260, nullable: false),
                    Path = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomRootEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntryCaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Directory = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false),
                    LastReferenced = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(260)", maxLength: 260, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntryCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThumbnailCaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false),
                    LastReferenced = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    PngData = table.Column<byte[]>(type: "BLOB SUB_TYPE BINARY", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThumbnailCaches", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_FileEntryCache_Path",
                table: "FileEntryCaches",
                columns: new[] { "Directory", "LastReferenced", "Name" });

            migrationBuilder.CreateIndex(
                name: "IDX_ThumbnailCache_Date",
                table: "ThumbnailCaches",
                column: "LastReferenced");

            migrationBuilder.CreateIndex(
                name: "IDX_ThumbnailCache_Path",
                table: "ThumbnailCaches",
                column: "Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomRootEntries");

            migrationBuilder.DropTable(
                name: "FileEntryCaches");

            migrationBuilder.DropTable(
                name: "ThumbnailCaches");
        }
    }
}

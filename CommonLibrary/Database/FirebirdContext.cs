﻿using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database;

public class FirebirdContext : DbContext
{

    private static readonly LoggerFactory loggerFactory = new (new ILoggerProvider[] { new NLogLoggerProvider() });

    private readonly FileInfo databaseFile;

    public DbSet<ThumbnailCache> ThumbnailCaches { get; set; }

    private FirebirdContext() { throw new NotSupportedException(); }

    internal FirebirdContext(FileInfo database)
    {
        databaseFile = database;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var stringBuilder = new StringBuilder();

        // 
        stringBuilder.Append($"database=localhost:{databaseFile.FullName};");
        stringBuilder.Append(@"user=sysdba;");
        stringBuilder.Append(@"password=admin;");

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .UseFirebird(stringBuilder.ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
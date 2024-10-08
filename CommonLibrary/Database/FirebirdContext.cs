using FirebirdSql.Data.FirebirdClient;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Database;

public class FirebirdContext : DbContext
{
    private readonly IServiceProvider? services;

    private readonly ILoggerFactory? loggerFactory;

    private readonly AssemblyResolver? resolver;

    private readonly FileInfo databaseFile;

    public DbSet<CustomRootEntry> CustomRootEntries { get; set; }

    public DbSet<ThumbnailCache> ThumbnailCaches { get; set; }

    public DbSet<FileEntryCache> FileEntryCaches { get; set; }

    public FirebirdContext(IDatabaseConfig config, IServiceProvider? services)
    {
        this.databaseFile = config.DatabaseFile;
        this.services = services;
        this.loggerFactory = this.services?.GetService<ILoggerFactory>();
        this.resolver = this.services?.GetService<AssemblyResolver>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var stringBuilder = new StringBuilder();

        // 接続文字列の作成 
        stringBuilder.Append(@"servertype=1;");
        stringBuilder.Append(@"clientlibrary=fbclient;");
        stringBuilder.Append($"database=localhost:{databaseFile.FullName};");
        stringBuilder.Append(@"user=sysdba;");
        stringBuilder.Append(@"password=admin;");
        var connstr = stringBuilder.ToString();

        if (!databaseFile.Exists)
        {
            // Firebirdプロセスでファイルを作ると、存在判定ができない
            using (var stream = databaseFile.Create()) { }
            Thread.Sleep(100);

            // DBファイルの作成
            FbConnection.CreateDatabase(connstr, 32768, true, true);
        }
        optionsBuilder.UseFirebird(connstr);

        if (loggerFactory != null)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

#if DEBUG
        optionsBuilder
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileEntryIndex>().ToSqlQuery("""
            SELECT
             "Id",
             "Directory",
             "LastReferenced",
             "Name",
             ROW_NUMBER() OVER (ORDER BY "Name" ASC) - 1 AS "Index" 
            FROM "FileEntryCaches" 
            ORDER BY
             "Name" ASC
            """);
    }

}

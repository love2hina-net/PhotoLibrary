using FirebirdSql.Data.FirebirdClient;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database;

public class FirebirdContext : DbContext
{

    private readonly LoggerFactory? loggerFactory;

    private readonly FileInfo databaseFile;

    public DbSet<ThumbnailCache> ThumbnailCaches { get; set; }

    public DbSet<FileEntryCache> FileEntryCaches { get; set; }

    private FirebirdContext() { throw new NotSupportedException(); }

    internal FirebirdContext(FileInfo database, LoggerFactory? logger)
    {
        loggerFactory = logger;
        databaseFile = database;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var stringBuilder = new StringBuilder();

        // 接続文字列の作成 
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
             "IndexHash",
             "Name",
             ROW_NUMBER() OVER (ORDER BY "Name" ASC) - 1 AS "Index" 
            FROM "FileEntryCaches" 
            ORDER BY
             "Name" ASC
            """);
    }

}

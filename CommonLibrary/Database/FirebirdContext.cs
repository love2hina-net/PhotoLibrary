using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database
{

    public class FirebirdContext : DbContext
    {

        private static readonly LoggerFactory loggerFactory = new (new ILoggerProvider[] { new NLogLoggerProvider() });

        public static string? Directory { get; set; }

        public DbSet<ThumbnailCache> ThumbnailCaches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var stringBuilder = new StringBuilder();
            var pathDirectory = Path.Combine(Directory!, @"love2hina\PhotoViewer");
            var pathDataFile = Path.Combine(pathDirectory, @"library.fdb");

            System.IO.Directory.CreateDirectory(pathDirectory);

            // 
            stringBuilder.Append($"database=localhost:{pathDataFile};");
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

    public class FirebirdContextFactory : IDesignTimeDbContextFactory<FirebirdContext>
    {

        public FirebirdContext CreateDbContext(string[] args)
        {
            FirebirdContext.Directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return new FirebirdContext();
        }

    }

}

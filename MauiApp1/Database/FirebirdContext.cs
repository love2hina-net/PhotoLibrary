using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoViewer.Database
{

    internal class FirebirdContext : DbContext
    {

        private static readonly LoggerFactory loggerFactory = new (new ILoggerProvider[] { new NLogLoggerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var stringBuilder = new StringBuilder();
            var pathDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, @"love2hina\PhotoViewer");
            var pathDataFile = Path.Combine(pathDirectory, @"library.fdb");

            Directory.CreateDirectory(pathDirectory);

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
        FirebirdContext IDesignTimeDbContextFactory<FirebirdContext>.CreateDbContext(string[] args)
        {
            return new FirebirdContext();
        }

    }

}

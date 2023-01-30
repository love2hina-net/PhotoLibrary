using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database;

public static class FirebirdContextFactory
{

    private static FileInfo? databaseFile;

    private static LoggerFactory? loggerFactory;

    public static FileInfo DatabaseFile
    {
        get => databaseFile ?? throw new InvalidOperationException();
    }

    public static void Initialize(string directory, bool designTime)
    {
        if (databaseFile == null)
        {
            Directory.CreateDirectory(directory);
            databaseFile = new FileInfo(Path.Combine(directory, @"LIBRARY.FDB"));

            if (!designTime)
            {
                loggerFactory = new(new ILoggerProvider[] { new NLogLoggerProvider() });
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static FirebirdContext Create() => new FirebirdContext(DatabaseFile, loggerFactory);

}

public class FirebirdContextDesignTimeFactory : IDesignTimeDbContextFactory<FirebirdContext>
{

    public FirebirdContext CreateDbContext(string[] args)
    {
        FirebirdContextFactory.Initialize(Environment.CurrentDirectory, true);
        return FirebirdContextFactory.Create();
    }

}

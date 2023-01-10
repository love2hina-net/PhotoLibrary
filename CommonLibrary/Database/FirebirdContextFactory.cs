using Microsoft.EntityFrameworkCore.Design;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database;

public static class FirebirdContextFactory
{

    private static FileInfo? databaseFile;

    public static FileInfo DatabaseFile
    {
        get => databaseFile ?? throw new InvalidOperationException();
    }

    public static void Initialize(string directory)
    {
        if (databaseFile == null)
        {
            Directory.CreateDirectory(directory);
            databaseFile = new FileInfo(Path.Combine(directory, @"library.fdb"));
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static FirebirdContext Create() => new FirebirdContext(DatabaseFile);

}

public class FirebirdContextDesignTimeFactory : IDesignTimeDbContextFactory<FirebirdContext>
{

    public FirebirdContext CreateDbContext(string[] args)
    {
        FirebirdContextFactory.Initialize(Environment.CurrentDirectory);
        return FirebirdContextFactory.Create();
    }

}

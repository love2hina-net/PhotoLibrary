using Microsoft.EntityFrameworkCore.Design;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database;

public interface IDatabaseConfig
{
    FileInfo DatabaseFile { get; }
}

public class DatabaseConfig : IDatabaseConfig
{

    public FileInfo DatabaseFile { get; private set; }

    public DatabaseConfig(string directory)
    {
        Directory.CreateDirectory(directory);
        DatabaseFile = new FileInfo(Path.Combine(directory, @"LIBRARY.FDB"));
    }

}

public class FirebirdContextDesignTimeFactory : IDesignTimeDbContextFactory<FirebirdContext>
{

    public FirebirdContext CreateDbContext(string[] args)
    {
        var config = new DatabaseConfig(Environment.CurrentDirectory);
        return new FirebirdContext(config, null);
    }

}

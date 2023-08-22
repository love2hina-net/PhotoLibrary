using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.InteropServices;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Database;

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

public static class NativeMethods
{

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetDllDirectory(
        [MarshalAs(UnmanagedType.LPWStr), In, Optional] string? lpPathName);

}

public class FirebirdContextDesignTimeFactory : IDesignTimeDbContextFactory<FirebirdContext>
{

    public FirebirdContext CreateDbContext(string[] args)
    {
        // for ef core migrations
        var dirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(FirebirdContextDesignTimeFactory))!.Location);
        NativeMethods.SetDllDirectory(dirPath);

        var config = new DatabaseConfig(Environment.CurrentDirectory);
        return new FirebirdContext(config, null);
    }

}

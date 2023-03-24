using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoViewer.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public class DirectoryCollection : ObservableCollection<DirectoryInfo>
{
    protected readonly ILogger logger;

    protected readonly Task enumrateTask;

    public DirectoryInfo? TargetDirectory { get; private set; }

    internal DirectoryCollection(
        IDbContextFactory<FirebirdContext> dbContextFactory,
        ILoggerFactory loggerFactory,
        DirectoryInfo? directory)
    {
        logger = loggerFactory.CreateLogger<DirectoryCollection>();

        TargetDirectory = directory;
        enumrateTask = Task.Run(() => Enumerate(dbContextFactory));
    }

    private void Enumerate(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        IEnumerable<DirectoryInfo> directories;

        if (TargetDirectory != null)
        {
            directories = TargetDirectory.EnumerateDirectories();
        }
        else
        {
            // ルート
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windowsはドライブの一覧を起点とする
                directories = from d in DriveInfo.GetDrives()
                              where d.IsReady
                              select d.RootDirectory;
            }
            else
            {
                // macOSはルートディレクトリを起点とする
                directories = new DirectoryInfo("/").EnumerateDirectories();
            }

            // カスタムルートを追加
            using var context = dbContextFactory.CreateDbContext();
            var rootDirInfo = (from entry in context.CustomRootEntries.AsNoTracking()
                               select new DirectoryInfo(entry.Path)).ToArray();

            directories = directories.Concat(rootDirInfo);
        }

        (from d in directories
         where d.IsAccessable()
         select d).ForEach(d => Add(d));
    }

}

[DeclareService(ServiceLifetime.Singleton)]
public class DirectoryCollectionFactory
{

    private readonly IDbContextFactory<FirebirdContext> dbContextFactory;

    private readonly ILoggerFactory loggerFactory;

    public DirectoryCollectionFactory(IDbContextFactory<FirebirdContext> dbContextFactory, ILoggerFactory loggerFactory)
    {
        this.dbContextFactory = dbContextFactory;
        this.loggerFactory = loggerFactory;
    }

    public DirectoryCollection Create(DirectoryInfo? directory = null)
        => new (dbContextFactory, loggerFactory, directory);

}

internal static class DirectoriInfoExtends
{

    public static bool IsAccessable(this DirectoryInfo directory)
    {
        try
        {
            return (directory.EnumerateFiles().FirstOrDefault() != null) || true;
        }
        catch (SystemException e) when (e is SecurityException || e is UnauthorizedAccessException)
        {
            return false;
        }
    }

}

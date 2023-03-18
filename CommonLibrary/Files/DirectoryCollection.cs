using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security;
using love2hina.Windows.MAUI.PhotoViewer.Common.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public class DirectoryCollection : ObservableCollection<DirectoryInfo>
{
    protected readonly ILogger logger;

    protected readonly Task enumrateTask;

    public DirectoryInfo? TargetDirectory { get; private set; }

    public DirectoryCollection(DirectoryInfo? directory = null)
    {
        logger = new LoggerFactory(new ILoggerProvider[] { new NLogLoggerProvider() }).CreateLogger<FileCollection>();

        TargetDirectory = directory;
        enumrateTask = Task.Run(Enumerate);
    }

    private void Enumerate()
    {
        IEnumerable<DirectoryInfo> directories;

        if (TargetDirectory != null)
        {
            directories = TargetDirectory.EnumerateDirectories();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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

        (from d in directories
         where d.IsAccessable()
         select d).ForEach(d => Add(d));
    }

}

internal static class DirectoriInfoExtends
{

    public static bool IsAccessable(this DirectoryInfo directory)
    {
        try
        {
            directory.EnumerateFiles().FirstOrDefault();
            return true;
        }
        catch (SystemException e) when (e is SecurityException || e is UnauthorizedAccessException)
        {
            return false;
        }
    }

}

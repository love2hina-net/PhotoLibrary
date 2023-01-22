using System.Runtime.InteropServices;
using System.Security;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class DirectoryView : CollectionView
{

    public DirectoryView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create(nameof(TargetDirectory), typeof(string), typeof(DirectoryView),
            propertyChanged: (b, o, n) => ((DirectoryView)b.BindingContext).OnPropertyChanged(nameof(Directories)));
    public string? TargetDirectory
    {
        get => (string?)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }

    public IEnumerable<string> Directories
    {
        get
        {
            IEnumerable<DirectoryInfo> directories;

            if (TargetDirectory != null)
            {
                directories = new DirectoryInfo(TargetDirectory).EnumerateDirectories();
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

            return from d in directories
                   where d.IsAccessable()
                   select d.FullName;
        }
    }

    async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var dir = e.CurrentSelection.FirstOrDefault() as string;
        await Shell.Current.GoToAsync($"directory?TargetDirectory={dir}", false);
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

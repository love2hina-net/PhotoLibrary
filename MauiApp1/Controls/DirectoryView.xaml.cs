using System.Runtime.InteropServices;

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
            if (TargetDirectory != null)
            {
                return Directory.EnumerateDirectories(TargetDirectory);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return from d in DriveInfo.GetDrives()
                       where d.IsReady
                       select d.RootDirectory.FullName;
            }
            else
            {
                return Directory.EnumerateDirectories("/");
            }
        }
    }

    async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var dir = e.CurrentSelection.FirstOrDefault() as string;
        await Shell.Current.GoToAsync($"directory?TargetDirectory={dir}", false);
    }

}

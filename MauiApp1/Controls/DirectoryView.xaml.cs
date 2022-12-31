using System.Runtime.InteropServices;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class DirectoryView : CollectionView
{

    public DirectoryView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create(nameof(TargetDirectory), typeof(DirectoryInfo), typeof(DirectoryView),
            propertyChanged: (b, o, n) => ((DirectoryView)b.BindingContext).OnPropertyChanged(nameof(Directories)));
    public DirectoryInfo TargetDirectory
    {
        get => (DirectoryInfo)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }

    public IEnumerable<DirectoryInfo> Directories
    {
        get
        {
            if (TargetDirectory != null)
            {
                return TargetDirectory.EnumerateDirectories();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return from d in DriveInfo.GetDrives()
                       where d.IsReady
                       select d.RootDirectory;
            }
            else
            {
                return (new DirectoryInfo("/")).EnumerateDirectories();
            }
        }
    }

    async void CollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        DirectoryInfo dir = e.CurrentSelection.FirstOrDefault() as DirectoryInfo;
        var navigationParameter = new Dictionary<string, object>
        {
            { "TargetDirectory", dir }
        };
        await Shell.Current.GoToAsync("//viewer/directory", false, navigationParameter);
    }

}

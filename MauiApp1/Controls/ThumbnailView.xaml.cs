namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class ThumbnailView : CollectionView
{

    public ThumbnailView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create(nameof(TargetDirectory), typeof(string), typeof(ThumbnailView),
            propertyChanged: (b, o, n) => ((ThumbnailView)b.BindingContext).OnPropertyChanged(nameof(Thumbnails)));
    public string? TargetDirectory
    {
        get => (string?)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }

    public IEnumerable<FileInfo> Thumbnails
    {
        get
        {
            if (TargetDirectory != null)
            {
                return new DirectoryInfo(TargetDirectory).EnumerateFiles();
            }
            else
            {
                return Enumerable.Empty<FileInfo>();
            }
        }
    }

}

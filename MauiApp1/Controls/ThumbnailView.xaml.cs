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

    public IEnumerable<Thumbnail> Thumbnails
    {
        get
        {
            if (TargetDirectory != null)
            {
                return from f in new DirectoryInfo(TargetDirectory).EnumerateFiles()
                       select new Thumbnail(f);
            }
            else
            {
                return Enumerable.Empty<Thumbnail>();
            }
        }
    }

}

public class Thumbnail
{

    public FileInfo File { get; private set; }

    internal Thumbnail(FileInfo info)
    {
        File = info;
    }

}

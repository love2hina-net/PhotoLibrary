using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;

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

    public IEnumerable<FileEntryCache> Thumbnails
    {
        get
        {
            if (TargetDirectory != null)
            {
                return new FileCollection(new DirectoryInfo(TargetDirectory));
            }
            else
            {
                return Enumerable.Empty<FileEntryCache>();
            }
        }
    }

}

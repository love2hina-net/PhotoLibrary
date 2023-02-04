using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class ThumbnailView : CollectionView
{

    protected IServiceProvider Services { get; private set; }

    public ThumbnailView()
    {
        Services = MauiProgram.Services;

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
                return new FileCollection(
                    Services.GetRequiredService<IDbContextFactory<FirebirdContext>>(),
                    Services.GetRequiredService<ILoggerFactory>(),
                    new DirectoryInfo(TargetDirectory));
            }
            else
            {
                return Enumerable.Empty<FileEntryCache>();
            }
        }
    }

}

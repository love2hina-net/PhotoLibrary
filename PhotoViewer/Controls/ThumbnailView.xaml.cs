using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

[DeclareService(ServiceLifetime.Transient)]
public partial class ThumbnailView : CollectionView
{

    protected readonly IServiceProvider services;

    public ThumbnailView(IServiceProvider services)
    {
        this.services = services;

        InitializeComponent();
    }

    public static ThumbnailView Create()
        => ControlLoader.Create<ThumbnailView>();

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
                    services.GetRequiredService<IDbContextFactory<FirebirdContext>>(),
                    services.GetRequiredService<ILoggerFactory>(),
                    new DirectoryInfo(TargetDirectory));
            }
            else
            {
                return Enumerable.Empty<FileEntryCache>();
            }
        }
    }

}

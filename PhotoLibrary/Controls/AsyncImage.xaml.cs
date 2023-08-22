using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Files;

namespace love2hina.Windows.MAUI.PhotoLibrary.Controls;

[DeclareService(ServiceLifetime.Transient)]
public partial class AsyncImage : Grid
{

    protected readonly ThumbnailLoader thumbnailLoader;

    public AsyncImage(ThumbnailLoader thumbnailLoader)
    {
        this.thumbnailLoader = thumbnailLoader;

        InitializeComponent();
    }

    public static AsyncImage Create()
        => ControlLoader.Create<AsyncImage>();

    public static readonly BindableProperty TargetFileProperty =
        BindableProperty.Create(nameof(TargetFile), typeof(FileInfo), typeof(AsyncImage),
            propertyChanged: (b, o, n) => ((AsyncImage)b).RefreshImage());
    public FileInfo? TargetFile
    {
        get => (FileInfo?)GetValue(TargetFileProperty);
        set => SetValue(TargetFileProperty, value);
    }

    protected void RefreshImage()
    {
        if (TargetFile != null)
        {
            var loadTask = thumbnailLoader.GetThumbnail(TargetFile)
                .ContinueWith((t) =>
                {
                    ThumbnailCache cache = t.Result;

                    if (cache.PngData != null)
                    {
                        var image = ImageSource.FromStream(() => new MemoryStream(cache.PngData));

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            thumbnail.Source = image;
                        });
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

}

using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class AsyncImage : Grid
{

    public static readonly string[] EXT = { ".jpg", ".png" };

    public AsyncImage()
    {
        InitializeComponent();
    }

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
        if ((TargetFile?.Exists ?? false) && (EXT.Contains(TargetFile.Extension.ToLower())))
        {
            var loadTask = ThumbnailLoader.GetThumbnail(TargetFile)
                .ContinueWith((t) =>
                {
                    ThumbnailCache cache = t.Result!;
                    var image = ImageSource.FromStream(() => new MemoryStream(cache.PngData!));

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        thumbnail.Source = image;
                    });
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

}

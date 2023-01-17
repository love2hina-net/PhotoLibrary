using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public static class ThumbnailLoader
{

    public static async Task<ThumbnailCache?> GetThumbnail(FileInfo file)
    {
        return await Task.Run(() =>
        {
            ThumbnailCache? cache = null;

            if (file.Exists)
            {
                using (var context = FirebirdContextFactory.Create())
                {
                    var query = from thumb in context.ThumbnailCaches.AsNoTracking()
                                where (thumb.IndexHash == file.FullName.GetHashCode()) && (thumb.Path == file.FullName)
                                select thumb;
                    cache = query.FirstOrDefault();
                }

                if (cache == null)
                {
                    // 読み込み処理
                    cache = LoadBitmapData(file);
                }
            }

            return cache;
        });
    }

    private static ThumbnailCache? LoadBitmapData(FileInfo file)
    {
        ThumbnailCache? cache = null;
        IImage? image = null;

        using (var stream = file.OpenRead())
        {
            image = SkiaImage.FromStream(stream)
                ?.Resize(256.0f, 256.0f, ResizeMode.Bleed, true);
        }

        if (image != null)
        {
            cache = new ThumbnailCache(file.FullName)
            {
                Width = (int)image.Width,
                Height = (int)image.Height,
                PngData = image.AsBytes(ImageFormat.Png)
            };

            using (var context = FirebirdContextFactory.Create())
            {
                context.ThumbnailCaches.Add(cache);
                context.SaveChanges();
            }
        }

        return cache;
    }

}

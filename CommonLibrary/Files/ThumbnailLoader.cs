using ImageMagick;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

[DeclareService(ServiceLifetime.Singleton)]
public class ThumbnailLoader
{

    protected readonly IDbContextFactory<FirebirdContext> dbContextFactory;

    public ThumbnailLoader(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public Task<ThumbnailCache?> GetThumbnail(FileInfo file)
    {
        return Task.Run(() =>
        {
            ThumbnailCache? cache = null;

            if (file.Exists)
            {
                using (var context = dbContextFactory.CreateDbContext())
                {
                    var query = from thumb in context.ThumbnailCaches.AsNoTracking()
                                where thumb.Path == file.FullName
                                select thumb;
                    cache = query.FirstOrDefault();

                    if (cache == null)
                    {
                        // 読み込み処理
                        cache = LoadBitmapData(context, file);
                    }
                    else
                    {
                        // 利用更新
                        cache.UpdateReference();
                        context.SaveChanges();
                    }
                }
            }

            return cache;
        });
    }

    private ThumbnailCache? LoadBitmapData(FirebirdContext context, FileInfo file)
    {
        ThumbnailCache? cache = null;
        IMagickImage? image = null;

        using (var stream = file.OpenRead())
        {
            image = new MagickImage(stream);
        }

        if (image != null)
        {
            // 縮小する
            var size = new MagickGeometry(256, 256);

            size.IgnoreAspectRatio = false;
            image.Resize(size);

            cache = new ThumbnailCache(file)
            {
                Width = (int)size.Width,
                Height = (int)size.Height,
                PngData = image.AsWriteBytes(MagickFormat.Png)
            };

            context.ThumbnailCaches.Add(cache);
            context.SaveChanges();
        }

        return cache;
    }
}

internal static class MagickImageExtensions
{

    internal static byte[]? AsWriteBytes(this IMagickImage target, MagickFormat format = MagickFormat.Png)
    {
        using (var stream = new MemoryStream())
        {
            target.Write(stream, format);
            return stream.ToArray();
        }
    }

}

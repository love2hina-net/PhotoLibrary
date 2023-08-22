using ImageMagick;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Database;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Files;

[DeclareService(ServiceLifetime.Singleton)]
public class ThumbnailLoader
{

    protected readonly IDbContextFactory<FirebirdContext> dbContextFactory;

    protected readonly ILogger<ThumbnailLoader> logger;

    public ThumbnailLoader(
        IDbContextFactory<FirebirdContext> dbContextFactory,
        ILoggerFactory loggerFactory)
    {
        this.dbContextFactory = dbContextFactory;
        logger = loggerFactory.CreateLogger<ThumbnailLoader>();
    }

    public Task<ThumbnailCache> GetThumbnail(FileInfo file)
    {
        return Task.Run(() =>
        {
            ThumbnailCache? cache = null;

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

            return cache;
        });
    }

    private ThumbnailCache LoadBitmapData(FirebirdContext context, FileInfo file)
    {
        IMagickImage? image = null;

        try
        {
            // ファイルを開く
            image = new MagickImage(file);

            // 縮小する
            image.Resize(new MagickGeometry(256, 256) { IgnoreAspectRatio = false });
        }
        catch (MagickException e)
        {
            logger.LogDebug(e, "Couldn't create a thumbnail, file: {file}.", file.FullName);
        }

        var cache = new ThumbnailCache(file)
        {
            Width = image?.Width,
            Height = image?.Height,
            PngData = image?.ToByteArray(MagickFormat.Png)
        };

        context.ThumbnailCaches.Add(cache);
        context.SaveChanges();

        return cache;
    }
}

﻿using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public static class ThumbnailLoader
{

    public static Task<ThumbnailCache?> GetThumbnail(FileInfo file)
    {
        return Task.Run(() =>
        {
            ThumbnailCache? cache = null;

            if (file.Exists)
            {
                using (var context = FirebirdContextFactory.Create())
                {
                    var query = from thumb in context.ThumbnailCaches.AsNoTracking()
                                where thumb.Path == file.FullName
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
            image = SkiaImage.FromStream(stream);
        }

        if (image != null)
        {
            // 縮小する
            var rate = Math.Min(Math.Min(256.0f / image.Width, 256.0f / image.Height), 1.0f);
            var size = new SizeF(image.Width * rate, image.Height * rate);

            using (var context = new SkiaBitmapExportContext((int)size.Width, (int)size.Height, 1))
            {
                context.Canvas.DrawImage(image, 0, 0, size.Width, size.Height);

                cache = new ThumbnailCache(file)
                {
                    Width = (int)size.Width,
                    Height = (int)size.Height,
                    PngData = context.AsBytes(ImageFormat.Png)
                };
            }

            using (var context = FirebirdContextFactory.Create())
            {
                context.ThumbnailCaches.Add(cache);
                context.SaveChanges();
            }
        }

        return cache;
    }

    internal static byte[]? AsBytes(this SkiaBitmapExportContext target, ImageFormat format = ImageFormat.Png, float quality = 1.0f)
    {
        if (target == null) return null;

        int skQuality = (int)(100.0f * quality);
        SKEncodedImageFormat skFormat = format switch
        {
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Bmp or ImageFormat.Gif or ImageFormat.Tiff => throw new PlatformNotSupportedException($"Skia does not support {format} format."),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };

        using (var stream = new MemoryStream())
        {
            using (var data = target.SKImage.Encode(skFormat, skQuality))
            {
                data.SaveTo(stream);
                return stream.ToArray();
            }
        }
    }

}

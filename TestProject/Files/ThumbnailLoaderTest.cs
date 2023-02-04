﻿using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Test.Files;

public class ThumbnailLoaderTest : Startup
{

    private IDbContextFactory<FirebirdContext> DbContextFactory { get; set; }

    private ThumbnailLoader ThumbnailLoader { get; set; }

    public ThumbnailLoaderTest(
        IDbContextFactory<FirebirdContext> dbContextFactory,
        ThumbnailLoader thumbnailLoader)
    {
        DbContextFactory = dbContextFactory;
        ThumbnailLoader = thumbnailLoader;
    }

    [Fact]
    public async void Test()
    {
        using (var context = DbContextFactory.CreateDbContext())
        {
            context.Database.Migrate();

            var testImage = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Resources\Images\20221224_110821834_iOS.jpg"));
            var cache =  await ThumbnailLoader.GetThumbnail(testImage);

            Assert.NotNull(cache);
        }
    }

}

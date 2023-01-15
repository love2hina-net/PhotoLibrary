using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Test.Files;

public class ThumbnailLoaderTest : Initializer
{

    [Fact]
    public async void Test()
    {
        using (var context = FirebirdContextFactory.Create())
        {
            context.Database.Migrate();

            var testImage = new FileInfo(Path.Combine(Environment.CurrentDirectory, @"Resources\Images\20221224_110821834_iOS.jpg"));
            var cache =  await ThumbnailLoader.GetThumbnail(testImage);

            Assert.NotNull(cache);
        }
    }

}

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Test.Files;

public class ThumbnailLoaderTest : Initializer
{

    [Fact]
    public void Test()
    {
        using (var context = FirebirdContextFactory.Create())
        {
            context.Database.Migrate();
        }
    }

}

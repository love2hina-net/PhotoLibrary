using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using Microsoft.Extensions.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoViewer.Test;

public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ThumbnailLoader>()
                .AddSingleton<IDatabaseConfig>(new DatabaseConfig(Environment.CurrentDirectory))
                .AddDbContextFactory<FirebirdContext>();
    }

}

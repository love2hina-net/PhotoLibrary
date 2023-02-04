using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoViewer.Test;

public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFromAssemblies()
                .AddSingleton<IDatabaseConfig>(new DatabaseConfig(Environment.CurrentDirectory))
                .AddDbContextFactory<FirebirdContext>();
    }

}

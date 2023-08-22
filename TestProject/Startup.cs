using love2hina.Windows.MAUI.PhotoLibrary.Common.Database;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoLibrary.Test;

public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFromAssemblies()
                .AddSingleton<IDatabaseConfig>(new DatabaseConfig(Environment.CurrentDirectory))
                .AddDbContextFactory<FirebirdContext>();
    }

}

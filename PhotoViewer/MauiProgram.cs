namespace love2hina.Windows.MAUI.PhotoViewer;

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Foldable;
using NLog.Extensions.Logging;

public static class MauiProgram
{

    // TODO: これなくしたい
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseFoldable()
            .Services.AddFromAssemblies()
                     .AddSingleton<IDatabaseConfig>(new DatabaseConfig(FileSystem.Current.AppDataDirectory))
                     .AddDbContextFactory<FirebirdContext>();

        // ログの設定
        builder.Logging.AddNLog();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddLogging();

        var app = builder.Build();
        Services = app.Services;
        return app;
    }
}

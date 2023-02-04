namespace love2hina.Windows.MAUI.PhotoViewer;

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Files;
using love2hina.Windows.MAUI.PhotoViewer.Controls;
using love2hina.Windows.MAUI.PhotoViewer.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Foldable;
using NLog.Extensions.Logging;

public static class MauiProgram
{

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
            .Services.AddSingleton<ThumbnailLoader>()
                     .AddTransient<ThumbnailView>()
                     .AddSingleton<InitialPage>()
                     .AddSingleton<RootShell>()
                     .AddSingleton<IDatabaseConfig>(new DatabaseConfig(FileSystem.Current.AppDataDirectory))
                     .AddDbContextFactory<FirebirdContext>();

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

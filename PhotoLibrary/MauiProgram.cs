namespace love2hina.Windows.MAUI.PhotoLibrary;

using love2hina.Windows.MAUI.PhotoLibrary.Common.Database;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Foldable;
using NLog.Extensions.Logging;

public static class MauiProgram
{

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

        return builder.Build();
    }
}

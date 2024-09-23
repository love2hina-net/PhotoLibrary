using ImageMagick;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Database;
using love2hina.Windows.MAUI.PhotoLibrary.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoLibrary;

public partial class App : Application
{

    protected readonly IServiceProvider services;

    protected readonly ILogger<App> logger;

    public App(IServiceProvider services)
    {
        this.services = services;

        // ロガーの取得
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        this.logger = loggerFactory.CreateLogger<App>();

        InitializeComponent();

        MainPage = services.GetRequiredService<InitialPage>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Title = @"ネコ写真部の大図書館 - a good library for photography club and cats.";

        return window;
    }

    protected override void OnStart()
    {
        base.OnStart();

        var initTask = Task.Run(() =>
        {
            try {
                var dbContextFactory = services.GetRequiredService<IDbContextFactory<FirebirdContext>>();

                MagickNET.Initialize();

                // DBの初期化
                using (var context = dbContextFactory.CreateDbContext())
                {
                    // DBテーブルを構成する
                    context.Database.Migrate();
                }

                // 初期化完了
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MainPage = services.GetRequiredService<RootShell>();
                });
            }
            catch (Exception e) {
                logger.LogError(e, "初期化エラー");

                // エラー表示
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var page = services.GetRequiredService<ErrorPage>();
                    page.ExceptionTitle = "初期化エラー";
                    page.ExceptionInfo = e;
                    MainPage = page;
                });
            }
        });
    }

}

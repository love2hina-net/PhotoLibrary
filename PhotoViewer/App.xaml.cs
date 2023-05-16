using ImageMagick;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Pages;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer;

public partial class App : Application
{

    protected readonly IServiceProvider services;

    public App(IServiceProvider services)
    {
        this.services = services;

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
        });
    }

}

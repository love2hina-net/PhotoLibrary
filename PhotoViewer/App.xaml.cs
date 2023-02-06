using love2hina.Windows.MAUI.PhotoViewer.Pages;

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

        var initPage = (InitialPage?)MainPage ?? throw new NullReferenceException();
        var initTask = Task.Run(() =>
        {
            initPage.Initialize();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = services.GetRequiredService<RootShell>();
            });
        });
    }

}

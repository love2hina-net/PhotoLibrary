using love2hina.Windows.MAUI.PhotoViewer.Pages;

namespace love2hina.Windows.MAUI.PhotoViewer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new InitialPage();
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
                MainPage = new RootShell();
            });
        });
    }

}

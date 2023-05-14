using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

[DeclareService(ServiceLifetime.Singleton)]
public partial class InitialPage : ContentPage
{

    public InitialPage()
    {
        InitializeComponent();
    }

}

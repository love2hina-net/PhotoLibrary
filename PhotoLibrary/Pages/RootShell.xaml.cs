using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoLibrary.Pages;

[DeclareService(ServiceLifetime.Singleton)]
public partial class RootShell : Shell
{
    public RootShell()
    {
        InitializeComponent();
    }
}

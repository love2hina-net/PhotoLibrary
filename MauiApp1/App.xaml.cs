namespace love2hina.Windows.MAUI.PhotoViewer;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

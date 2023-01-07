using love2hina.Windows.MAUI.PhotoViewer.Database;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		base.OnStart();

		await Task.Run(() =>
		{
			// DBの初期化
			using (var context = new FirebirdContext())
			{
                // DBテーブルを構成する
                context.Database.Migrate();
            }
		});
	}

}

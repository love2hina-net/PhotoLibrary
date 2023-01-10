using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
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

		FirebirdContextFactory.Initialize(FileSystem.Current.AppDataDirectory);

		await Task.Run(() =>
		{
			// DBの初期化
			using (var context = FirebirdContextFactory.Create())
			{
                // DBテーブルを構成する
                context.Database.Migrate();
            }
		});
	}

}

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

public partial class InitialPage : ContentPage
{

    public InitialPage()
    {
        InitializeComponent();
    }

    public void Initialize()
    {
        // コンテキストの初期化
        FirebirdContextFactory.Initialize(FileSystem.Current.AppDataDirectory);

        // DBの初期化
        using (var context = FirebirdContextFactory.Create())
        {
            // DBテーブルを構成する
            context.Database.Migrate();
        }
    }

}

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

[DeclareService(ServiceLifetime.Singleton)]
public partial class InitialPage : ContentPage
{

    protected readonly IDbContextFactory<FirebirdContext> dbContextFactory;

    public InitialPage(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;

        InitializeComponent();
    }

    public void Initialize()
    {
        // DB�̏�����
        using (var context = dbContextFactory.CreateDbContext())
        {
            // DB�e�[�u�����\������
            context.Database.Migrate();
        }
    }

}

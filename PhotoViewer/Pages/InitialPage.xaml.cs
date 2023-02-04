using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

public partial class InitialPage : ContentPage
{

    protected IDbContextFactory<FirebirdContext> DbContextFactory { get; private set; }

    public InitialPage(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;

        InitializeComponent();
    }

    public void Initialize()
    {
        // DB�̏�����
        using (var context = DbContextFactory.CreateDbContext())
        {
            // DB�e�[�u�����\������
            context.Database.Migrate();
        }
    }

}

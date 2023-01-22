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
        // �R���e�L�X�g�̏�����
        FirebirdContextFactory.Initialize(FileSystem.Current.AppDataDirectory);

        // DB�̏�����
        using (var context = FirebirdContextFactory.Create())
        {
            // DB�e�[�u�����\������
            context.Database.Migrate();
        }
    }

}

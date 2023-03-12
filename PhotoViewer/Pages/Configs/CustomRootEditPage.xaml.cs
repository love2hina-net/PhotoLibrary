using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoViewer.Controls;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages.Configs;

[DeclareService(ServiceLifetime.Transient)]
[QueryProperty(nameof(EntryId), "EntryId")]
public partial class CustomRootEditPage : ContentPage
{

    protected readonly IDbContextFactory<FirebirdContext> m_dbContextFactory;

    private CustomRootEntry? m_rootEntry = null;

    public CustomRootEditPage(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        InitializeComponent();

        m_dbContextFactory = dbContextFactory;
    }

    public static CustomRootEditPage Create()
        => ControlLoader.Create<CustomRootEditPage>();

    public string? EntryId
    {
        get => m_rootEntry?.Id.ToString();
        set
        {
            int? id = (value != null)? int.Parse(value) : null;

            if (id == null || id < 0)
            {
                RootEntry = new CustomRootEntry() { Id = -1 };
            }
            else
            {
                Task.Run(() =>
                {
                    using (var context = m_dbContextFactory.CreateDbContext())
                    {
                        RootEntry = (from r in context.CustomRootEntries.AsNoTracking()
                                     where r.Id == id
                                     select r).First();
                    }
                });
            }
        }
    }

    public CustomRootEntry? RootEntry
    {
        get => m_rootEntry;
        set
        {
            if (m_rootEntry != value)
            {
                m_rootEntry = value;
                OnPropertyChanged(nameof(RootEntry));
            }
        }
    }

}

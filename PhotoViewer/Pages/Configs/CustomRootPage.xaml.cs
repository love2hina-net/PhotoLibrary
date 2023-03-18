using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoViewer.Common.Extensions;
using love2hina.Windows.MAUI.PhotoViewer.Controls;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages.Configs;

[DeclareService(ServiceLifetime.Transient)]
[QueryProperty(nameof(UpdatedEntryId), "UpdatedEntryId")]
public partial class CustomRootPage : ContentPage
{
    protected readonly IDbContextFactory<FirebirdContext> m_dbContextFactory;

    private readonly ObservableCollection<CustomRootEntry> m_rootEntries = new(); 

    public CustomRootPage(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        InitializeComponent();
        m_dbContextFactory = dbContextFactory;

        Routing.RegisterRoute("config/edit", typeof(CustomRootEditPage));

        Update();
    }

    public static CustomRootPage Create()
        => ControlLoader.Create<CustomRootPage>();

    public ObservableCollection<CustomRootEntry> RootEntries
    {
        get => m_rootEntries;
    }

    public string? UpdatedEntryId
    {
        get => null;
        set
        {
            int? id = (value != null) ? int.Parse(value) : null;

            if (id != null && id >= 0)
            {
                Update();
            }
        }
    }

    private Task Update() => Task.Run(() =>
    {
        m_rootEntries.Clear();
        using (var context = m_dbContextFactory.CreateDbContext())
        {
            var newItem = new[] { new CustomRootEntry() { Id = -1, Name = "新規追加" } };
            newItem.Concat(context.CustomRootEntries.AsNoTracking()).ForEach(i => m_rootEntries.Add(i));
        }
    });

    async void CollectionView_OnTapped(object sender, TappedEventArgs args)
    {
        var entry = rootEntriesView.SelectedItem as CustomRootEntry;
        if (entry != null)
        {
            await Shell.Current.GoToAsync($"edit?EntryId={entry.Id}", false);
        }
    }

}

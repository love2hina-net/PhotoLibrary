using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoViewer.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoViewer.Controls;
using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages.Configs;

[DeclareService(ServiceLifetime.Transient)]
public partial class CustomRootPage : ContentPage
{

    private List<CustomRootEntry>? rootEntries = null;

    public CustomRootPage(IDbContextFactory<FirebirdContext> dbContextFactory)
    {
        InitializeComponent();

        Routing.RegisterRoute("config/edit", typeof(CustomRootEditPage));

        Task.Run(() =>
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var newItem = new[] { new CustomRootEntry() { Id = -1, Name = "新規追加" } };
                RootEntries = newItem.Concat(context.CustomRootEntries.AsNoTracking()).ToList();
            }
        });
    }

    public static CustomRootPage Create()
        => ControlLoader.Create<CustomRootPage>();

    public List<CustomRootEntry>? RootEntries
    {
        get => rootEntries;
        set
        {
            rootEntries = value;
            OnPropertyChanged(nameof(RootEntries));
        }
    }

    async void CollectionView_OnTapped(object sender, TappedEventArgs args)
    {
        var entry = rootEntriesView.SelectedItem as CustomRootEntry;
        if (entry != null)
        {
            await Shell.Current.GoToAsync($"edit?EntryId={entry.Id}", false);
        }
    }

}

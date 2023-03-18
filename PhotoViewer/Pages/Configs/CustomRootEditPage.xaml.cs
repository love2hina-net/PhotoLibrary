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

    // 純粋なバックフィールド
    private readonly CustomRootEntry m_rootEntry = new CustomRootEntry();

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
                m_rootEntry.Id = -1;
                Name = string.Empty;
                Path = string.Empty;
            }
            else
            {
                Task.Run(() =>
                {
                    using (var context = m_dbContextFactory.CreateDbContext())
                    {
                        var entry= (from r in context.CustomRootEntries.AsNoTracking()
                                    where r.Id == id
                                    select r).First();

                        m_rootEntry.Id = entry.Id;
                        Name = entry!.Name;
                        Path = entry!.Path;
                    }
                });
            }
        }
    }

    public string? Name
    {
        get => m_rootEntry.Name;
        set
        {
            if (value != null && m_rootEntry.Name != value)
            {
                m_rootEntry.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string? Path
    {
        get => m_rootEntry?.Path;
        set
        {
            if (value != null && m_rootEntry.Path != value)
            {
                m_rootEntry.Path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        // チェック
        if (string.IsNullOrEmpty(Name))
        {
            await DisplayAlert("Notice", "名前を入力してください", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(Path))
        {
            await DisplayAlert("Notice", "パスを入力してください", "OK");
            return;
        }
        else if (!global::System.IO.Path.Exists(Path))
        {
            await DisplayAlert("Notice", "指定されたパスが無効です", "OK");
            return;
        }

        CustomRootEntry entry = await Task.Run(() =>
        {
            CustomRootEntry entry;

            using (var context = m_dbContextFactory.CreateDbContext())
            {
                if (m_rootEntry.Id < 0)
                {
                    // 新規作成
                    entry = new CustomRootEntry() { Name = Name, Path = Path };
                    context.CustomRootEntries.Add(entry);
                }
                else
                {
                    // 更新
                    entry = (from r in context.CustomRootEntries
                             where r.Id == m_rootEntry.Id
                             select r).First();

                    entry.Name = Name;
                    entry.Path = Path;
                }

                context.SaveChanges();
            }

            return entry;
        });
        await Shell.Current.GoToAsync($"..?UpdatedEntryId={entry.Id}");
    }

}

using love2hina.Windows.MAUI.PhotoLibrary.Common.Database;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoLibrary.Controls;

[DeclareService(ServiceLifetime.Transient)]
public partial class ThumbnailView : CollectionViewEx
{

    protected readonly IServiceProvider services;

    private IList<FileEntryCache> fileEntries = Utils.EmptyFileEntryList;

    public ThumbnailView(IServiceProvider services)
    {
        this.services = services;

        InitializeComponent();
    }

    public static ThumbnailView Create()
        => ControlLoader.Create<ThumbnailView>();

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create(nameof(TargetDirectory), typeof(string), typeof(ThumbnailView),
            propertyChanged: (bindable, oldValue, newValue) => ((ThumbnailView)bindable.BindingContext).TargetDirectory_Changed(oldValue as string, newValue as string));
    public string? TargetDirectory
    {
        get => (string?)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }
    protected void TargetDirectory_Changed(string? oldValue, string? newValue)
    {
        if (newValue != null)
        {
            fileEntries = new FileCollection(
                services.GetRequiredService<IDbContextFactory<FirebirdContext>>(),
                services.GetRequiredService<ILoggerFactory>(),
                new DirectoryInfo(newValue));
        }
        else
        {
            fileEntries = Utils.EmptyFileEntryList;
        }
        OnPropertyChanged(nameof(Thumbnails));
    }

    public IEnumerable<FileEntryCache> Thumbnails
    {
        get => fileEntries;
    }

    private async void ThumbnailView_Selected(object sender, SelectedEventArgs e)
    {
        if (e.SelectedItem is FileEntryCache file)
        {
            var param = new Dictionary<string, object> {
                { "FileEntries", fileEntries },
                { "SelectedIndex", fileEntries.IndexOf(file) }
            };
            await Shell.Current.GoToAsync("item", false, param);
        }
    }

}

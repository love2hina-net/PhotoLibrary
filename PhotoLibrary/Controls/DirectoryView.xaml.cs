using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Files;

namespace love2hina.Windows.MAUI.PhotoLibrary.Controls;

[DeclareService(ServiceLifetime.Transient)]
public partial class DirectoryView : CollectionViewEx
{

    private readonly DirectoryCollectionFactory factory;

    public DirectoryView(DirectoryCollectionFactory factory)
    {
        this.factory = factory;

        InitializeComponent();
    }

    public static DirectoryView Create()
        => ControlLoader.Create<DirectoryView>();

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create(nameof(TargetDirectory), typeof(string), typeof(DirectoryView),
            propertyChanged: (b, o, n) => ((DirectoryView)b.BindingContext).OnPropertyChanged(nameof(Directories)));
    public string? TargetDirectory
    {
        get => (string?)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }

    public DirectoryCollection Directories
    {
        get
        {
            var path = TargetDirectory;
            return factory.Create((path != null) ? new DirectoryInfo(path) : null);
        }
    }

    private async void DirectoryView_Tapped(object sender, TappedEventArgs e)
    {
        var dir = SelectedItem as DirectoryInfo;
        if (dir != null)
        {
            await Shell.Current.GoToAsync($"directory?TargetDirectory={dir.FullName}", false);
        }
    }

}

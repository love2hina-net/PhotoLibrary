using love2hina.Windows.MAUI.PhotoViewer.Common.Files;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class DirectoryView : CollectionView
{

    public DirectoryView()
    {
        InitializeComponent();
    }

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
            return new DirectoryCollection((path != null) ? new DirectoryInfo(path) : null);
        }
    }

    async void DirectoryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var dir = e.CurrentSelection.FirstOrDefault() as DirectoryInfo;
        if (dir != null)
        {
            SelectedItem = null;
            await Shell.Current.GoToAsync($"directory?TargetDirectory={dir.FullName}", false);
        }
    }

}

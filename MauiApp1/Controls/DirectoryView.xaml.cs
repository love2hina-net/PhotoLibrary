using System.Collections.Generic;

namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class DirectoryView : CollectionView
{

    public DirectoryView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TargetDirectoryProperty =
        BindableProperty.Create("TargetDirectory", typeof(DirectoryInfo), typeof(DirectoryView), null);
    public DirectoryInfo TargetDirectory
    {
        get => (DirectoryInfo)GetValue(TargetDirectoryProperty);
        set => SetValue(TargetDirectoryProperty, value);
    }

    public IEnumerable<DirectoryInfo> Directories
    {
        get => TargetDirectory?.EnumerateDirectories() ??
            (from d in DriveInfo.GetDrives()
             where d.IsReady
             select d.RootDirectory);
    }

    async void CollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        DirectoryInfo dir = e.CurrentSelection.FirstOrDefault() as DirectoryInfo;
        var navigationParameter = new Dictionary<string, object>
        {
            { "TargetDirectory", dir }
        };
        Console.WriteLine($"DirectoryView changed: {dir}");
        await Shell.Current.GoToAsync("//Test", false, navigationParameter);
    }

}

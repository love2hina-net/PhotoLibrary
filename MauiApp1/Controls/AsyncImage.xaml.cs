namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class AsyncImage : View
{

    public AsyncImage()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TargetFileProperty =
        BindableProperty.Create(nameof(TargetFile), typeof(FileInfo), typeof(AsyncImage),
            propertyChanged: (b, o, n) => ((AsyncImage)b.BindingContext).RefreshImage());
    public FileInfo? TargetFile
    {
        get => (FileInfo?)GetValue(TargetFileProperty);
        set => SetValue(TargetFileProperty, value);
    }

    protected void RefreshImage()
    {
    }

}

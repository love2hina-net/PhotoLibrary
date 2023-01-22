namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class AsyncImage : Grid
{

    public AsyncImage()
    {
        InitializeComponent();

        //var image = ImageSource.FromFile(@"noimage.png");
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

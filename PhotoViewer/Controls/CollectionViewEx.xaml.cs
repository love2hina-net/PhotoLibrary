namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public partial class CollectionViewEx : CollectionView
{

    public CollectionViewEx()
    {
        InitializeComponent();
    }

    public event EventHandler<TappedEventArgs>? Tapped;

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        => Tapped?.Invoke(this, e);

}

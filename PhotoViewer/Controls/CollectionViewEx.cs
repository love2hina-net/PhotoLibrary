namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public class CollectionViewEx : CollectionView
{

    public CollectionViewEx()
    {
        var rec = new TapGestureRecognizer();
        rec.Tapped += TapGestureRecognizer_Tapped;
        GestureRecognizers.Add(rec);
    }

    public event EventHandler<TappedEventArgs>? Tapped;

    private void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        => Tapped?.Invoke(this, e);

}

namespace love2hina.Windows.MAUI.PhotoLibrary.Controls;

public class CollectionViewEx : CollectionView
{

    public event EventHandler<SelectedEventArgs>? Selected;

    public CollectionViewEx()
    {
        SelectionChanged += CollectionViewEx_SelectionChanged;
    }

    private void CollectionViewEx_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var eventSelected = Selected;

        if (SelectedItem != null && eventSelected != null)
        {
            var args = new SelectedEventArgs(e.CurrentSelection);
            eventSelected.Invoke(this, args);

            if (args.IsEventTrapped)
            {
                // TODO: MAUI BUG, Windowsでは選択が解除されない
                // https://github.com/dotnet/maui/issues/10025#issuecomment-2329849049
                Task.Delay(100).ContinueWith((prev) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        SelectedItem = null;
                    });
                });
            }
        }
    }

}

public class SelectedEventArgs : EventArgs
{

    public IReadOnlyList<object> SelectedItems { get; }

    public bool IsEventTrapped { get; set; } = true;

    internal SelectedEventArgs(IReadOnlyList<object> selected)
    {
        SelectedItems = selected;
    }

    public object? SelectedItem => SelectedItems.FirstOrDefault();

}

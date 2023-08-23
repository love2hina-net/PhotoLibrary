using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;

namespace love2hina.Windows.MAUI.PhotoLibrary.Pages;

public partial class ItemViewerPage : ContentPage, IQueryAttributable
{

    private int _selectedIndex = -1;

    private bool _buttonVisible = false;

    public ItemViewerPage()
    {
        FileEntries = Utils.EmptyFileEntryList;

        InitializeComponent();
        back.Opacity = 0.0;
        forward.Opacity = 0.0;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        FileEntries = (IList<FileEntryCache>)query["FileEntries"];
        SelectedIndex = (int)query["SelectedIndex"];
    }

    public IList<FileEntryCache> FileEntries { get; set; }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex == value)
            {
                // 何もしない
            }
            else if (value < 0)
            {
                _selectedIndex = value;
                TargetFile = null;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(IsBackEnabled));
                OnPropertyChanged(nameof(IsForwardEnabled));
                OnPropertyChanged(nameof(TargetFile));
            }
            else if (0 <= value && value < FileEntries.Count)
            {
                _selectedIndex = value;
                TargetFile = FileEntries[SelectedIndex].Path;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(IsBackEnabled));
                OnPropertyChanged(nameof(IsForwardEnabled));
                OnPropertyChanged(nameof(TargetFile));
            }
        }
    }

    public FileInfo? TargetFile
    {
        get;
        protected set;
    }

    public bool IsBackEnabled => SelectedIndex > 0;

    public bool IsForwardEnabled => SelectedIndex < FileEntries.Count - 1;

    private void BackButton_Clicked(object sender, EventArgs e) => SelectedIndex -= 1;
    private void ForwardButton_Clicked(object sender, EventArgs e) => SelectedIndex += 1;

    public bool IsButtonVisible
    {
        get => _buttonVisible;
        set
        {
            if (_buttonVisible != value)
            {
                _buttonVisible = value;
                back.Opacity = value ? 1.0 : 0.0;
                forward.Opacity = value ? 1.0 : 0.0;
                OnPropertyChanged(nameof(IsButtonVisible));
            }
        }
    }

    private void Button_PointerEntered(object sender, PointerEventArgs e) => IsButtonVisible = true;
    private void Button_PointerExited(object sender, PointerEventArgs e) => IsButtonVisible = false;

}

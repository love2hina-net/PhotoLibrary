using love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;

namespace love2hina.Windows.MAUI.PhotoLibrary.Pages;

public partial class ItemViewerPage : ContentPage, IQueryAttributable
{

    private int _selectedIndex = -1;

    public ItemViewerPage()
    {
        FileEntries = Utils.EmptyFileEntryList;

        InitializeComponent();
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
                OnPropertyChanged(nameof(TargetFile));
            }
            else if (0 <= value && value < FileEntries.Count)
            {
                _selectedIndex = value;
                TargetFile = FileEntries[SelectedIndex].Path;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(TargetFile));
            }
        }
    }

    public FileInfo? TargetFile
    {
        get;
        protected set;
    }

    private void BackButton_Clicked(object sender, EventArgs e) => SelectedIndex -= 1;
    private void NextButton_Clicked(object sender, EventArgs e) => SelectedIndex += 1;

}

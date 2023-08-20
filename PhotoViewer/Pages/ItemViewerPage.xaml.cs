using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

[QueryProperty(nameof(FileEntries), "FileEntries"),
 QueryProperty(nameof(SelectedIndex), "SelectedIndex")]
public partial class ItemViewerPage : ContentPage
{

    private string? m_fileTarget;

    public ItemViewerPage()
    {
        FileEntries = Utils.EmptyFileEntryList;

        InitializeComponent();
    }

    public IList<FileEntryCache> FileEntries { get; set; }

    public int SelectedIndex { get; set; }


    public string? TargetFile
    {
        get => m_fileTarget;
        set
        {
            if (m_fileTarget != value)
            {
                m_fileTarget = value;
                OnPropertyChanged(nameof(TargetFile));
            }
        }
    }

}

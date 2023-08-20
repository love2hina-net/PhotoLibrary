using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

public partial class ItemViewerPage : ContentPage, IQueryAttributable
{

    public ItemViewerPage()
    {
        FileEntries = Utils.EmptyFileEntryList;

        InitializeComponent();
    }

    // TODO: 微妙
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        FileEntries = (IList<FileEntryCache>)query["FileEntries"];
        SelectedIndex = (int)query["SelectedIndex"];
        TargetFile_Changed();
    }

    public IList<FileEntryCache> FileEntries { get; set; }

    public int SelectedIndex { get; set; }

    protected void TargetFile_Changed()
    {
        if (0 <= SelectedIndex && SelectedIndex < FileEntries.Count)
        {
            TargetFile = FileEntries[SelectedIndex].Path.FullName;
            OnPropertyChanged(nameof(TargetFile));
        }
    }
 
    public string? TargetFile
    {
        get;
        protected set;
    }

}

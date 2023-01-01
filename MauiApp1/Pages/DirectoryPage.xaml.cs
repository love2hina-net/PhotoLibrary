namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

[QueryProperty(nameof(TargetDirectory), "TargetDirectory")]
public partial class DirectoryPage : ContentPage
{

    private string? m_dirTarget;

    public DirectoryPage()
    {
        InitializeComponent();
    }

    public string? TargetDirectory
    {
        get => m_dirTarget;
        set
        {
            if (m_dirTarget != value)
            {
                m_dirTarget = value;
                OnPropertyChanged(nameof(TargetDirectory));
            }
        }
    }

}
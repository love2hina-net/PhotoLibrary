namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

[QueryProperty(nameof(TargetDirectory), "TargetDirectory")]
public partial class DirectoryPage : ContentPage
{

    private DirectoryInfo m_dirTarget;

    public DirectoryPage()
    {
        InitializeComponent();
    }

    public DirectoryInfo TargetDirectory
    {
        get => m_dirTarget;
        set
        {
            Console.WriteLine($"DirectoryPage changed: {value}");
            if (m_dirTarget != value)
            {
                m_dirTarget = value;
                OnPropertyChanged(nameof(TargetDirectory));
            }
        }
    }

}
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;

namespace love2hina.Windows.MAUI.PhotoLibrary.Pages;

[DeclareService(ServiceLifetime.Singleton)]
public partial class ErrorPage : ContentPage
{

    private string _exceptionTitle = "Error";

    private Exception? _exceptionInfo;

    public ErrorPage()
    {
        InitializeComponent();
    }
    
    public string ExceptionTitle
    {
        get { return _exceptionTitle; }
        set
        {
            if (_exceptionTitle != value)
            {
                _exceptionTitle = value;
                OnPropertyChanged(nameof(ExceptionTitle));
            }
        }
    }

    public Exception? ExceptionInfo {
        get { return _exceptionInfo; }
        set {
            if (_exceptionInfo != value)
            {
                _exceptionInfo = value;
                OnPropertyChanged(nameof(ExceptionInfo));
                OnPropertyChanged(nameof(ExceptionMessage));
            }
            
        }
    }

    public string ExceptionMessage {
        get {
            if (ExceptionInfo != null) {
                return $"""
                    {ExceptionInfo.Message}

                    Type:
                    {ExceptionInfo.GetType().FullName}

                    StackTrace:
                    {ExceptionInfo.StackTrace}
                    """;
            }
            else
            {
                return string.Empty;
            }
        }
    }

}

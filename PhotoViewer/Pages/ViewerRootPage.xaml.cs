namespace love2hina.Windows.MAUI.PhotoViewer.Pages;

public partial class ViewerRootPage : ContentPage
{
    public ViewerRootPage()
    {
        InitializeComponent();

        Routing.RegisterRoute("viewer/directory", typeof(DirectoryPage));
    }

    async void DirectoryView_Clicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("//viewer/directory");
    }

}
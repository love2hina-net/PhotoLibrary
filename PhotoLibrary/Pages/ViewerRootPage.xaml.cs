namespace love2hina.Windows.MAUI.PhotoLibrary.Pages;

public partial class ViewerRootPage : ContentPage
{
    public ViewerRootPage()
    {
        InitializeComponent();

        Routing.RegisterRoute("viewer/directory", typeof(DirectoryPage));
        Routing.RegisterRoute("viewer/item", typeof(ItemViewerPage));
    }

    async void DirectoryView_Clicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("//viewer/directory");
    }

}
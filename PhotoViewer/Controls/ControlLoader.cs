namespace love2hina.Windows.MAUI.PhotoViewer.Controls;

public static class ControlLoader
{

    public static T Create<T>() where T : notnull
        => MauiProgram.Services.GetRequiredService<T>();

}

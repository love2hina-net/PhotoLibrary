using love2hina.Windows.MAUI.PhotoViewer.Common.Database;

namespace love2hina.Windows.MAUI.PhotoViewer.Test;

public class Initializer
{

    static Initializer()
    {
        FirebirdContextFactory.Initialize(Environment.CurrentDirectory, true);
    }

}

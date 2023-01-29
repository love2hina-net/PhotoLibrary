using Microsoft.EntityFrameworkCore;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Keyless]
public class FileEntryIndex : FileEntryCache
{

    public int Index { get; set; }

}

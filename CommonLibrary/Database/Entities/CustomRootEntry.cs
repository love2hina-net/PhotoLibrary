using System.ComponentModel.DataAnnotations;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Database.Entities;

public class CustomRootEntry
{

    public virtual int Id { get; set; }

    /** エントリ名 */
    [Required]
    [MaxLength(260)]
    public virtual string Name { get; set; } = string.Empty;

    /** パス */
    [Required]
    [MaxLength(1024)]
    public virtual string Path { get; set; } = string.Empty;

}

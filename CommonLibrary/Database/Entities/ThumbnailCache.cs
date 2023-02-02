using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Index(nameof(Path), Name = @"IDX_ThumbnailCache_Path")]
[Index(nameof(LastReferenced), Name = @"IDX_ThumbnailCache_Date")]
public class ThumbnailCache
{

    public virtual int Id { get; set; }

    /** ファイルフルパス */
    [Required]
    [MaxLength(1024)]
    public virtual string Path { get; set; }

    /** キャッシュを使用した日付 */
    [Required]
    public virtual DateTime LastReferenced { get; set; }

    public virtual int? Width { get; set; }

    public virtual int? Height { get; set; }

    public virtual byte[]? PngData { get; set; }

    public ThumbnailCache()
    {
        Path = string.Empty;
    }

    public ThumbnailCache(FileInfo file)
    {
        Path = file.FullName;
        LastReferenced = DateTime.Now;
    }

}

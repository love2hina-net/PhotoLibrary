using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Index(nameof(IndexHash), nameof(Path), Name = @"IDX_ThumbnailCache_Path")]
[Index(nameof(LastReferenced), Name = @"IDX_ThumbnailCache_Date")]
public class ThumbnailCache
{

    public virtual int Id { get; set; }

    /**
     * インデックスハッシュ値.
     * 
     * フルパスのハッシュ値
     */
    [Required]
    public virtual int IndexHash { get; set; }

    /** ファイルフルパス */
    [Required]
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

    public ThumbnailCache(string path)
    {
        Path = path;
        IndexHash = path.GetHashCode();
        LastReferenced = DateTime.Now;
    }

}

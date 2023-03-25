using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Index(nameof(Directory), nameof(LastReferenced), nameof(Name), Name = @"IDX_FileEntryCache_Path")]
public class FileEntryCache
{

    public virtual int Id { get; set; }

    /** ディレクトリパス */
    [Required]
    [MaxLength(1024)]
    public virtual string Directory { get; set; } = string.Empty;

    /** キャッシュを使用した日付 */
    [Required]
    public virtual DateTime LastReferenced { get; set; } = DateTime.MinValue;

    /** ファイル名 */
    [Required]
    [MaxLength(260)]
    public virtual string Name { get; set; } = string.Empty;

    [NotMapped]
    public virtual FileInfo Path
    {
        get => new FileInfo(System.IO.Path.Combine(Directory, Name));
    }

    public FileEntryCache()
    {
    }

    public FileEntryCache(DirectoryInfo parent, FileInfo file)
    {
        Directory = parent.FullName;
        Name = file.Name;
    }

}

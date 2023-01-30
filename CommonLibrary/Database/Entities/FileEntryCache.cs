using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Index(nameof(Directory), nameof(Name), Name = @"IDX_FileEntryCache_Path")]
public class FileEntryCache
{

    public virtual int Id { get; set; }

    /** ディレクトリパス */
    [Required]
    [MaxLength(1024)]
    public virtual string Directory { get; set; }

    /** ファイル名 */
    [Required]
    [MaxLength(260)]
    public virtual string Name { get; set; }

    /**
     * インデックスハッシュ値.
     * 
     * フルパスのハッシュ値
     */
    public virtual int IndexHash { get; set; }

    public FileEntryCache()
    {
        Directory = string.Empty;
        Name = string.Empty;
    }

    public FileEntryCache(DirectoryInfo parent, FileInfo file)
    {
        Directory = parent.FullName;
        Name = file.Name;
        IndexHash = file.FullName.GetHashCode();
    }

}

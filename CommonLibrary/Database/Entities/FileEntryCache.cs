using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;

[Index(nameof(Directory), nameof(Path), Name = @"IDX_FileEntryCache_Path")]
public class FileEntryCache
{

    public virtual int Id { get; set; }

    /** ディレクトリパス */
    [Required]
    public virtual string Directory { get; set; }

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

    public FileEntryCache()
    {
        Directory = string.Empty;
        Path = string.Empty;
    }

    public FileEntryCache(DirectoryInfo parent, FileInfo file)
    {
        Directory = parent.FullName;
        Path = file.FullName;
        IndexHash = Path.GetHashCode();
    }

}

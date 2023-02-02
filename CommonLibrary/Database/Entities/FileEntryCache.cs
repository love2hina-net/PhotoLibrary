﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [NotMapped]
    public virtual FileInfo Path
    {
        get => new FileInfo(System.IO.Path.Combine(Directory, Name));
    }

    public FileEntryCache()
    {
        Directory = string.Empty;
        Name = string.Empty;
    }

    public FileEntryCache(DirectoryInfo parent, FileInfo file)
    {
        Directory = parent.FullName;
        Name = file.Name;
    }

}

using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.ComponentModel;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public class FileCollection :
    ICollection<FileEntryCache>, IReadOnlyCollection<FileEntryCache>,
    IList<FileEntryCache>, IReadOnlyList<FileEntryCache>, System.Collections.IList,
    IEnumerable<FileEntryCache>, INotifyCollectionChanged, INotifyPropertyChanged
{

    protected const int FETCH_COUNT = 10;

    protected readonly ILogger logger;

    protected readonly Task enumrateTask;

    protected IDbContextFactory<FirebirdContext> DbContextFactory { get; private set; }

    public DirectoryInfo TargetDirectory { get; private set; }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public FileCollection(
        IDbContextFactory<FirebirdContext> dbContextFactory,
        ILoggerFactory loggerFactory,
        DirectoryInfo directory)
    {
        DbContextFactory = dbContextFactory;
        logger = loggerFactory.CreateLogger<FileCollection>();

        TargetDirectory = directory;
        enumrateTask = Task.Run(Enumerate);
    }

    private void Enumerate()
    {
        var list = new List<FileEntryCache>();

        foreach (var file in TargetDirectory.EnumerateFiles())
        {
            list.Add(new FileEntryCache(TargetDirectory, file));

            if (list.Count >= FETCH_COUNT)
            {
                AddFiles(list);
            }
        }

        AddFiles(list);
    }

    private void AddFiles(List<FileEntryCache> list)
    {

        if (list.Count > 0)
        {
            logger.LogTrace(list, "Requested append items");

            using (var context = DbContextFactory.CreateDbContext())
            {
                // 追加するエンティティの抽出
                var addEntities = (from newEntry in list
                                   join existsEntry in context.FileEntryCaches 
                                    on new { newEntry.Directory, newEntry.Name } equals new { existsEntry.Directory, existsEntry.Name } into joinGroup
                                   from joinedEntry in joinGroup.DefaultIfEmpty(null)
                                   where joinedEntry == null
                                   select newEntry).ToArray();

                logger.LogTrace(addEntities, "Actual append items");

                if (addEntities.Length > 0) {
                    // 追加する
                    context.FileEntryCaches.AddRange(addEntities);
                    context.SaveChanges();

                    var arrayId = (from entity in addEntities
                                   select entity.Id).ToArray();

                    // 追加した要素のインデックス番号を取得
                    var indexedEntities = (from entity in context.Set<FileEntryIndex>()
                                            .FromSqlInterpolated($"""
                                                SELECT
                                                 "Id",
                                                 "Directory",
                                                 "Name",
                                                 ROW_NUMBER() OVER (ORDER BY "Name" ASC) - 1 AS "Index" 
                                                FROM "FileEntryCaches" 
                                                WHERE
                                                 "Directory" = {TargetDirectory.FullName} 
                                                ORDER BY
                                                 "Name" ASC
                                                """)
                                           where arrayId.Contains(entity.Id)
                                           orderby entity.Index ascending
                                           select entity).AsNoTracking().ToArray();
                    logger.LogTrace(indexedEntities, "Changed items");

                    // 変更を通知
                    logger.LogDebug("Notify changed: {index}", indexedEntities[0].Index);
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, indexedEntities, indexedEntities[0].Index));
                }
            }

            list.Clear();
        }
    }

    protected FileEntryCache Get(int index)
    {
        logger.LogInformation("Get item: {index}", index);

        using (var context = DbContextFactory.CreateDbContext())
        {
            return (from entity in context.Set<FileEntryIndex>()
                     .FromSqlInterpolated($"""
                        SELECT
                         "Id",
                         "Directory",
                         "Name",
                         ROW_NUMBER() OVER (ORDER BY "Name" ASC) - 1 AS "Index" 
                        FROM "FileEntryCaches" 
                        WHERE
                         "Directory" = {TargetDirectory.FullName} 
                        ORDER BY
                         "Name" ASC
                        """)
                     where entity.Index == index
                     select entity).First();
        }
    }

    public bool IsReadOnly => false;

    public bool IsFixedSize => false;

    public bool IsSynchronized => false;

    public object SyncRoot => logger;

    #region 要素参照系

    public int Count
    {
        get
        {
            int count;
            using (var context = DbContextFactory.CreateDbContext())
            {
                count = (from entity in context.FileEntryCaches
                         where entity.Directory == TargetDirectory.FullName
                         select entity).Count();
            }

            logger.LogInformation("Get count: {count}", count);
            return count;
        }
    }

    public FileEntryCache this[int index]
    {
        get => Get(index);
        set => throw new NotSupportedException();
    }

    object? System.Collections.IList.this[int index]
    {
        get => Get(index);
        set => throw new NotSupportedException();
    }

    public bool Contains(FileEntryCache item)
    {
        // TODO
        throw new NotImplementedException();
    }

    bool System.Collections.IList.Contains(object? item) => Contains((FileEntryCache)item!);

    public int IndexOf(FileEntryCache item)
    {
        // TODO
        throw new NotImplementedException();
    }

    int System.Collections.IList.IndexOf(object? item) => IndexOf((FileEntryCache)item!);

    #endregion

    #region 要素追加・削除系

    public void Clear()
    {
        // TODO
        throw new NotImplementedException();
    }

    int AddImpl(FileEntryCache item)
    {
        // TODO
        throw new NotImplementedException();
    }

    public void Add(FileEntryCache item) => AddImpl(item);

    int System.Collections.IList.Add(object? item) => AddImpl((FileEntryCache)item!);

    public void Insert(int index, FileEntryCache item)
    {
        // TODO
        throw new NotImplementedException();
    }

    void System.Collections.IList.Insert(int index, object? item) => Insert(index, (FileEntryCache)item!);

    public bool Remove(FileEntryCache item)
    {
        // TODO
        throw new NotImplementedException();
    }

    void System.Collections.IList.Remove(object? item) => Remove((FileEntryCache)item!);

    public void RemoveAt(int index)
    {
        // TODO
        throw new NotImplementedException();
    }

    #endregion

    private IEnumerator<FileEntryCache> GetEnumeratorImpl()
    {
        using (var context = DbContextFactory.CreateDbContext())
        {
            return context.FileEntryCaches.FromSqlInterpolated($"""
                SELECT
                 "Id",
                 "Directory",
                 "Name",
                FROM "FileEntryCaches"
                """).AsNoTracking().GetEnumerator();
        }
    }

    public IEnumerator<FileEntryCache> GetEnumerator() => GetEnumeratorImpl();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumeratorImpl();

    private void CopyToImpl(FileEntryCache[] array, int index)
    {
        // TODO
        throw new NotImplementedException();
    }

    public void CopyTo(FileEntryCache[] array, int index) => CopyToImpl(array, index);

    void System.Collections.ICollection.CopyTo(Array array, int index) => CopyToImpl((FileEntryCache[])array, index);

}

internal static class LoggerExtension
{

    public static void LogTrace(this ILogger logger, IReadOnlyList<FileEntryCache> entities, string? message = null)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace(@">>> {message} {type} array dump begin, count: {count}.", message ?? "", nameof(FileEntryCache), entities.Count);

            foreach (var entity in entities)
            {
                logger.LogTrace(@"--- Name: {Name}", entity.Name);
            }

            logger.LogTrace(@"<<< {message} {type} array dump end.", message ?? "", nameof(FileEntryCache));
        }
    }

    public static void LogTrace(this ILogger logger, IReadOnlyList<FileEntryIndex> entities, string? message = null)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace(@">>> {message} {type} array dump begin, count: {count}.", message ?? "", nameof(FileEntryIndex), entities.Count);

            foreach (var entity in entities)
            {
                logger.LogTrace(@"--- Index: {Index}, Name: {Name}", entity.Index, entity.Name);
            }

            logger.LogTrace(@"<<< {message} {type} array dump end.", message ?? "", nameof(FileEntryIndex));
        }
    }

}

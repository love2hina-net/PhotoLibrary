using FirebirdSql.Data.FirebirdClient;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database;
using love2hina.Windows.MAUI.PhotoViewer.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace love2hina.Windows.MAUI.PhotoViewer.Common.Files;

public class FileCollection :
    ICollection<FileEntryCache>, IReadOnlyCollection<FileEntryCache>,
    IList<FileEntryCache>, IReadOnlyList<FileEntryCache>, System.Collections.IList,
    IEnumerable<FileEntryCache>, INotifyCollectionChanged, INotifyPropertyChanged
{

    protected const int FETCH_COUNT = 10;

    protected readonly ILogger logger;

    protected readonly Task enumrateTask;

    public DirectoryInfo TargetDirectory { get; private set; }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public FileCollection(DirectoryInfo directory)
    {
        logger = new LoggerFactory(new ILoggerProvider[] { new NLogLoggerProvider() }).CreateLogger<FileCollection>();

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

            using (var context = FirebirdContextFactory.Create())
            {
                // 追加するエンティティの抽出
                var addEntities = (from newEntry in list
                                   join existsEntry in context.FileEntryCaches 
                                    on new { newEntry.Directory, newEntry.Path } equals new { existsEntry.Directory, existsEntry.Path } into joinGroup
                                   from joinedEntry in joinGroup.DefaultIfEmpty(null)
                                   where joinedEntry == null
                                   select newEntry).ToArray();

                logger.LogTrace(addEntities, "Actual append items");

                if (addEntities.Length > 0) {
                    // 追加する
                    context.FileEntryCaches.AddRange(addEntities);
                    context.SaveChanges();

                    // 追加した要素のインデックス番号を取得
                    var queryBuilder = new StringBuilder();
                    var queryParam = new FbParameter[addEntities.Length + 1];
                    queryParam[0] = new FbParameter(@"@directory", TargetDirectory.FullName);
                    queryBuilder.Append("""
                    SELECT
                     "Id",
                     "Directory",
                     "IndexHash",
                     "Path",
                     "Index"
                    FROM (
                     SELECT
                      "Id",
                      "Directory",
                      "IndexHash",
                      "Path",
                      ROW_NUMBER() OVER (ORDER BY "Path" ASC) - 1 AS "Index"
                     FROM "FileEntryCaches"
                     WHERE
                      "Directory" = @directory
                     ORDER BY
                      "Path")
                    WHERE
                     "Id" IN (
                    """);
                    for (int i = 0; i < addEntities.Length; i++)
                    {
                        if (i > 0) queryBuilder.Append(@", ");
                        queryBuilder.Append($"@id{i}");
                        queryParam[i + 1] = new FbParameter($"@id{i}", addEntities[i].Id);
                    }
                    queryBuilder.Append("""
                    )
                    ORDER BY
                     "Index" ASC
                    """);

                    var indexed = context.Set<FileEntryIndex>().
                        FromSqlRaw(queryBuilder.ToString(), queryParam).AsNoTracking().ToArray();

                    logger.LogTrace(indexed, "Changed items");

                    // 変更を通知
                    logger.LogInformation("Notify changed: {index}", indexed[0].Index);
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, indexed, indexed[0].Index));
                }
            }

            list.Clear();
        }
    }

    protected FileEntryCache Get(int index)
    {
        logger.LogInformation("Get item: {index}", index);

        using (var context = FirebirdContextFactory.Create())
        {
            return (from entity in context.Set<FileEntryIndex>()
                     .FromSqlInterpolated($"""
                        SELECT
                         "Id",
                         "Directory",
                         "IndexHash",
                         "Path",
                         ROW_NUMBER() OVER (ORDER BY "Path" ASC) - 1 AS "Index" 
                        FROM "FileEntryCaches" 
                        WHERE
                         "Directory" = {TargetDirectory.FullName} 
                        ORDER BY
                         "Path" ASC
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
            using (var context = FirebirdContextFactory.Create())
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
        using (var context = FirebirdContextFactory.Create())
        {
            return context.FileEntryCaches.FromSqlInterpolated($"""
                SELECT
                 "Id",
                 "Directory",
                 "IndexHash",
                 "Path",
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
                logger.LogTrace(@"--- Path: {Path}", entity.Path);
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
                logger.LogTrace(@"--- Index: {Index}, Path: {Path}", entity.Index, entity.Path);
            }

            logger.LogTrace(@"<<< {message} {type} array dump end.", message ?? "", nameof(FileEntryIndex));
        }
    }

}

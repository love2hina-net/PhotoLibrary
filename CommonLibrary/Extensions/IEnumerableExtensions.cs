namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Extensions;

public static class IEnumerableExtensions
{

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

}

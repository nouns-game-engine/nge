namespace Nouns.Editor;

internal static class OrderExtensions
{
    public static int TrySortByOrder<T>(T x, T y)
    {
        if (x == null || y == null)
            return int.MaxValue;
        var lo = x.GetType().GetCustomAttributes(typeof(OrderAttribute), true);
        var ro = y.GetType().GetCustomAttributes(typeof(OrderAttribute), true);
        if (lo.Length != 1 && ro.Length != 1)
            return int.MaxValue;
        var lx = lo.Length == 1 ? ((OrderAttribute)lo[0]).Order : int.MaxValue;
        var rx = ro.Length == 1 ? ((OrderAttribute)ro[0]).Order : int.MaxValue;
        return lx.CompareTo(rx);
    }
}
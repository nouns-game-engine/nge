namespace NGE.Core
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this Type type) => typeof(T).IsAssignableFrom(type);
    }
}

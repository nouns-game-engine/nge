using System;

namespace NGE.Editor
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this Type type) => typeof(T).IsAssignableFrom(type);
    }
}

using System.Collections.Generic;
using Android.Runtime;

namespace Xamdroid.Extensions
{
    public static class EnumerableExtension
    {
        public static JavaList<T> ToJavaList<T>(this IEnumerable<T> source)
        {
            var list = new JavaList<T>();
            foreach (var item in source) list.Add(item);
            return list;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Game.Utility
{
    public static class ListExtensions
    {
        public static T Random<T>(this IList<T> list)
        {
            return list.Count switch
            {
                0 => throw new IndexOutOfRangeException("List needs at least one entry to call Random()"),
                1 => list[0],
                _ => list[UnityEngine.Random.Range(0, list.Count)]
            };
        }

        public static T RandomOrDefault<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list.Random();
        }

        public static T PopLast<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var t = list[^1];

            list.RemoveAt(list.Count - 1);

            return t;
        }
    }
}
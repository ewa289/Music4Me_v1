using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Music4Me.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> Paged<T>(this IEnumerable<T> source, int pageSize)
        {
            if (source == null) yield break;

            using (var enumerator = source.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var currentPage = new List<T>(pageSize) {
                        enumerator.Current
                    };

                    while (currentPage.Count < pageSize && enumerator.MoveNext()) {
                        currentPage.Add(enumerator.Current);
                    }
                    yield return new ReadOnlyCollection<T>(currentPage);
                }
            }
        }
    }
}

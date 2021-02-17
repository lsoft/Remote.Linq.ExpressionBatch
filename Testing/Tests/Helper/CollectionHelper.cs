using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Helper
{
    public static class CollectionHelper
    {
        //
        // Summary:
        //     Returns the second element of a sequence.
        //
        // Parameters:
        //   source:
        //     The System.Collections.Generic.IEnumerable`1 to return the second element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The second element in the specified sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty or does not contains the second element.
        public static TSource Second<TSource>(
            this IEnumerable<TSource> source
            )
        {
            return source.Nth(1);
        }

        //
        // Summary:
        //     Returns the third element of a sequence.
        //
        public static TSource Third<TSource>(
            this IEnumerable<TSource> source
            )
        {
            return source.Nth(2);
        }

        //
        // Summary:
        //     Returns the Nth element of a sequence.
        //
        public static TSource Nth<TSource>(
            this IEnumerable<TSource> source,
            int index
            )
        {
            return source.Skip(index).First();
        }

        public static IEnumerable<TSource> ForwardBeyond<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate
            )
        {
            return source.SkipWhile((i) => !predicate(i)).Skip(1);
        }
    }
}

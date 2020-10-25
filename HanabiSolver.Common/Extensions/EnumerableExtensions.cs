using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Common.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TResult> AsEnumerable<TResult>(this TResult element)
		{
			return new TResult[] { element };
		}

		public static IEnumerable<TSource> ExceptAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			return source.Where((element, i) => i != index);
		}

		public static int Median(this IEnumerable<int> source)
		{
			return (int)source.Middle().Average();
		}

		public static IEnumerable<TSource> Middle<TSource>(this IEnumerable<TSource> source)
		{
			if (source.Count() % 2 == 1)
				return source.Skip(source.Count() / 2).Take(1);
			else
				return source.Skip(source.Count() / 2 - 1).Take(2);
		}

		public static bool None<TSource>(this IEnumerable<TSource> source)
		{
			return !source.Any();
		}

		public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return !source.Any(predicate);
		}
	}
}

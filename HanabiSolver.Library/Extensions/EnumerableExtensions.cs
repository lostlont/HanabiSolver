using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Extensions
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

using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Extensions
{
	public static class EnumerableExtensions
	{
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			return source
				.Select((e, i) => new { Element = e, Index = i })
				.Single(x => x.Element?.Equals(element) == true)
				.Index;
		}
	}
}

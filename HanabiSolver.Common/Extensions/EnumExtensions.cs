using HanabiSolver.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Common.Extensions
{
	public static class EnumExtensions
	{
		public static TEnum? Next<TEnum>(this TEnum value)
			where TEnum : struct, Enum
		{
			return NextIn(value, EnumUtils.Values<TEnum>());
		}

		public static TEnum? Previous<TEnum>(this TEnum value)
			where TEnum : struct, Enum
		{
			return NextIn(value, EnumUtils.Values<TEnum>().Reverse());
		}

		private static TEnum? NextIn<TEnum>(this TEnum value, IEnumerable<TEnum> inValues)
			where TEnum : struct, Enum
		{
			var comparer = EqualityComparer<TEnum>.Default;

			var next = inValues
				.SkipWhile(e => !comparer.Equals(e, value))
				.Skip(1)
				.Cast<TEnum?>()
				.FirstOrDefault();

			return next;
		}
	}
}

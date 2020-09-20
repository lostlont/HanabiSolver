using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Utils
{
	public static class EnumUtils
	{
		public static IEnumerable<TEnum> Values<TEnum>()
			where TEnum : Enum
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}

		public static TEnum? Next<TEnum>(TEnum value)
			where TEnum : struct, Enum
		{
			return NextIn(value, Values<TEnum>());
		}

		public static TEnum? Previous<TEnum>(TEnum value)
			where TEnum : struct, Enum
		{
			return NextIn(value, Values<TEnum>().Reverse());
		}

		private static TEnum? NextIn<TEnum>(TEnum value, IEnumerable<TEnum> inValues)
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

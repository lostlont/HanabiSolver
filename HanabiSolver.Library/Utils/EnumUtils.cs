using System;
using System.Collections.Generic;

namespace HanabiSolver.Library.Utils
{
	public static class EnumUtils
	{
		public static IEnumerable<TEnum> Values<TEnum>()
			where TEnum : Enum
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Utils;
using System.Collections.Generic;

namespace HanabiSolver.Library.Tests.Utils
{
	public class EnumUtilsTests
	{
		private enum Test
		{
			A,
			B,
			C,
		}

		public void ValuesProvidesValuesList()
		{
			var values = EnumUtils.Values<Test>();

			var expectedValues = new List<Test>
			{
				Test.A,
				Test.B,
				Test.C,
			};
			values.Should().BeEquivalentTo(expectedValues);
		}
	}
}

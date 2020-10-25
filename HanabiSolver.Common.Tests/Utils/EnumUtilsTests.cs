using FluentAssertions;
using HanabiSolver.Common.Utils;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Common.Tests.Utils
{
	public class EnumUtilsTests
	{
		private enum Test
		{
			A,
			B,
			C,
		}

		[Fact]
		public void ValuesProvidesValuesList()
		{
			var values = EnumUtils.Values<Test>();

			var expectedValues = new List<Test>
			{
				Test.A,
				Test.B,
				Test.C,
			};
			values.Should().Equal(expectedValues);
		}
	}
}

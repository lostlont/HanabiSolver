using FluentAssertions;
using HanabiSolver.Library.Utils;
using System.Collections.Generic;
using Xunit;

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
			values.Should().BeEquivalentTo(expectedValues);
		}

		[Fact]
		public void NextProvidesSequentiallyNext()
		{
			var next = EnumUtils.Next(Test.A);

			next.Should().Be(Test.B);
		}

		[Fact]
		public void NextProvidesNothingForLast()
		{
			var next = EnumUtils.Next(Test.C);

			next.Should().BeNull();
		}
	}
}

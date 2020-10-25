using FluentAssertions;
using HanabiSolver.Common.Extensions;
using Xunit;

namespace HanabiSolver.Common.Tests.Extensions
{
	public class EnumExtensionsTests
	{
		private enum Test
		{
			A,
			B,
			C,
		}

		[Fact]
		public void NextProvidesSequentiallyNext()
		{
			var next = Test.A.Next();

			next.Should().Be(Test.B);
		}

		[Fact]
		public void NextProvidesNothingForLast()
		{
			var next = Test.C.Next();

			next.Should().BeNull();
		}

		[Fact]
		public void PreviousProvidesSequentiallyPrevious()
		{
			var previous = Test.B.Previous();

			previous.Should().Be(Test.A);
		}

		[Fact]
		public void PreviousProvidesNothingForFirst()
		{
			var previous = Test.A.Previous();

			previous.Should().BeNull();
		}
	}
}

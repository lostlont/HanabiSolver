using FluentAssertions;
using HanabiSolver.Common.Extensions;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Common.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void MiddleReturnsMiddleElementForOddAmount()
		{
			var chars = new List<char> { 'a', 'b', 'c' };

			var middle = chars.Middle();

			var expectedValues = new List<char> { 'b' };
			middle.Should().Equal(expectedValues);
		}

		[Fact]
		public void MiddleReturnsTwoMiddleElementsForEvenAmount()
		{
			var chars = new List<char> { 'a', 'b', 'c', 'd' };

			var middle = chars.Middle();

			var expectedValues = new List<char> { 'b', 'c' };
			middle.Should().Equal(expectedValues);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Common.Extensions;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Common.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void MedianReturnsMiddleElementForOddAmount()
		{
			var values = new List<int> { 1, 3, 10 };

			var median = values.Median();

			median.Should().Be(3);
		}

		[Fact]
		public void MedianReturnsAverageOfMiddleElementsForEvenAmount()
		{
			var values = new List<int> { 1, 3, 10, 30 };

			var median = values.Median();

			median.Should().Be((3 + 10) / 2);
		}
	}
}

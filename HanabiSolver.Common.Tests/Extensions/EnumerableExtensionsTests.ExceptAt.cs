using FluentAssertions;
using HanabiSolver.Common.Extensions;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Common.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void ExceptAtSkipsTheElementAtIndexUsed()
		{
			var characters = new List<char> { 'a', 'b', 'c' };

			var values = characters.ExceptAt(1);

			var expectedValues = new List<char> { 'a', 'c' };
			values.Should().Equal(expectedValues);
		}
	}
}

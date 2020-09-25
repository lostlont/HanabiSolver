using FluentAssertions;
using HanabiSolver.Library.Extensions;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Library.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void AsEnumerableHasTheSameElements()
		{
			var character = 'c';

			var enumerable = character.AsEnumerable();

			var expectedValues = new List<char> { 'c' };
			enumerable.Should().BeEquivalentTo(expectedValues);
		}
	}
}

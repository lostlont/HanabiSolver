using FluentAssertions;
using HanabiSolver.Library.Extensions;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Library.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void ExistingAsEnumerableReturnsTheSameElements()
		{
			var character = (char?)'c';

			var enumerable = character.ExistingAsEnumerable();

			var expectedValues = new List<char> { 'c' };
			enumerable.Should().Equal(expectedValues);
		}

		[Fact]
		public void ExistingAsEnumerableReturnsEmptyForNoValue()
		{
			var character = (char?)null;

			var enumerable = character.ExistingAsEnumerable();

			var expectedValues = new List<char> { };
			enumerable.Should().Equal(expectedValues);
		}
	}
}

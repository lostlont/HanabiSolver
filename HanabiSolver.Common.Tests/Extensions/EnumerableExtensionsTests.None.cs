using FluentAssertions;
using HanabiSolver.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Common.Tests.Extensions
{
	public partial class EnumerableExtensionsTests
	{
		[Fact]
		public void NoneIsTrueForEmpty()
		{
			var empty = Enumerable.Empty<char>();

			var none = empty.None();

			none.Should().BeTrue();
		}

		[Fact]
		public void NoneIsFalseForNonEmpty()
		{
			var some = new List<char> { 'a' };

			var none = some.None();

			none.Should().BeFalse();
		}

		[Fact]
		public void NoneWithPredicateIsTrueForEmpty()
		{
			var empty = Enumerable.Empty<char>();

			var none = empty.None(c => c == 'x');

			none.Should().BeTrue();
		}

		[Fact]
		public void NoneWithPredicateIsTrueForListFulfillingThePredicate()
		{
			var some = new List<char> { 'a', 'b', 'c' };

			var none = some.None(c => c == 'x');

			none.Should().BeTrue();
		}

		[Fact]
		public void NoneWithPredicateIsFalseForListFailingThePredicate()
		{
			var some = new List<char> { 'a', 'x', 'c' };

			var none = some.None(c => c == 'x');

			none.Should().BeFalse();
		}
	}
}

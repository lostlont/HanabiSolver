using FluentAssertions;
using HanabiSolver.Library.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Extensions
{
	public class EnumerableExtensionsTests
	{
		[Fact]
		public void AsEnumerableHasTheSameElements()
		{
			var character = 'c';

			var enumerable = character.AsEnumerable();

			var expectedValues = new List<char> { 'c' };
			enumerable.Should().BeEquivalentTo(expectedValues);
		}

		[Fact]
		public void ExceptAtSkipsTheElementAtIndexUsed()
		{
			var characters = new List<char> { 'a', 'b', 'c' };

			var values = characters.ExceptAt(1);

			var expectedValues = new List<char> { 'a', 'c' };
			values.Should().BeEquivalentTo(expectedValues);
		}

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

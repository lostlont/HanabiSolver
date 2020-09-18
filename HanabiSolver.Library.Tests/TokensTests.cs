using FluentAssertions;
using HanabiSolver.Library.Game;
using System;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public class TokensTests
	{
		[Fact]
		public void ConstructorSetsMaxAmount()
		{
			var tokens = new Tokens(3);

			tokens.MaxAmount.Should().Be(3);
		}

		[Fact]
		public void ConstructorSetsAmount()
		{
			var tokens = new Tokens(3);

			tokens.Amount.Should().Be(3);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public void ConstructorThrowsForNonPositiveMaxAmount(int maxAmount)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Tokens(maxAmount));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(4)]
		public void ConstructorThrowsForOutOfRangeAmount(int amount)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Tokens(3, amount));
		}

		[Fact]
		public void RemoveDecreasesAmount()
		{
			var tokens = new Tokens(3);

			tokens.Remove();

			tokens.Amount.Should().Be(2);
		}

		[Fact]
		public void RemoveFromEmptyDoesNotChange()
		{
			var tokens = new Tokens(3, 0);

			tokens.Remove();

			tokens.Amount.Should().Be(0);
		}

		[Fact]
		public void AddIncreasesAmount()
		{
			var tokens = new Tokens(3, 0);

			tokens.Add();

			tokens.Amount.Should().Be(1);
		}

		[Fact]
		public void AddToFullDoesNotChange()
		{
			var tokens = new Tokens(3);

			tokens.Add();

			tokens.Amount.Should().Be(3);
		}
	}
}

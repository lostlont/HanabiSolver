using FluentAssertions;
using HanabiSolver.Library.Game;
using System;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
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
		public void UseDecreasesAmount()
		{
			var tokens = new Tokens(3);

			tokens.Use();

			tokens.Amount.Should().Be(2);
		}

		[Fact]
		public void UseOnEmptyDoesNothing()
		{
			var tokens = new Tokens(3, 0);

			tokens.Use();

			tokens.Amount.Should().Be(0);
		}

		[Fact]
		public void ReplenishIncreasesAmount()
		{
			var tokens = new Tokens(3, 0);

			tokens.Replenish();

			tokens.Amount.Should().Be(1);
		}

		[Fact]
		public void ReplenishFullDoesNothing()
		{
			var tokens = new Tokens(3);

			tokens.Replenish();

			tokens.Amount.Should().Be(3);
		}
	}
}

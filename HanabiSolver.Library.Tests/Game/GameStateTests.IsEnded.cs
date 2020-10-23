using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using HanabiSolver.Library.Utils;
using Moq;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameStateTests
	{
		[Fact]
		public void IsNotEndedNormally()
		{
			var gameState = new GameStateBuilder().Build();

			gameState.IsEnded.Should().BeFalse();
		}

		[Theory]
		[InlineData(1, 0, false)]
		[InlineData(1, 1, true)]
		[InlineData(2, 1, false)]
		[InlineData(2, 2, true)]
		[InlineData(3, 0, false)]
		[InlineData(3, 2, false)]
		[InlineData(3, 3, true)]
		[InlineData(10, 0, false)]
		[InlineData(10, 9, false)]
		[InlineData(10, 10, true)]
		public void IsEndedWhenFuseTokensAreFull(int tokenMaxAmount, int tokenAmount, bool isEnded)
		{
			var fuseTokens = new Mock<ITokens>(MockBehavior.Strict);
			fuseTokens
				.Setup(t => t.MaxAmount)
				.Returns(tokenMaxAmount);
			fuseTokens
				.Setup(t => t.Amount)
				.Returns(tokenAmount);
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					FuseTokens = fuseTokens.Object,
				}.Build(),
			}.Build();

			gameState.IsEnded.Should().Be(isEnded);
		}

		[Fact]
		public void IsEndedWhenAllFivesArePlayed()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = EnumUtils.Values<Suite>()
						.ToDictionary(
							suite => suite,
							suite => BuildPile(suite)),
				}.Build(),
			}.Build();

			gameState.IsEnded.Should().BeTrue();
		}

		[Fact]
		public void IsNotEndedWhenAllButOneFivesArePlayed()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = EnumUtils.Values<Suite>()
						.ToDictionary(
							suite => suite,
							suite => BuildPile(suite, suite == Suite.White ? Number.Four : Number.Five)),
				}.Build(),
			}.Build();

			gameState.IsEnded.Should().BeFalse();
		}

		private IPile BuildPile(Suite suite) => BuildPile(suite, Number.Five);

		private IPile BuildPile(Suite suite, Number topNumber)
		{
			var result = new Mock<IPile>(MockBehavior.Strict);
			result
				.Setup(p => p.Top)
				.Returns(new Card(suite, topNumber));
			return result.Object;
		}
	}
}

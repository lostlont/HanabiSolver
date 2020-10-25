using FluentAssertions;
using HanabiSolver.Common.Utils;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameManagerTests
	{
		[Fact]
		public void ScoreReturnsZeroInitially()
		{
			var gameState = new GameStateBuilder().Build();
			var gameManager = new GameManager(gameState);

			gameManager.Score.Should().Be(0);
		}

		[Fact]
		public void ScoreReturnsTopCardsValueForOnePile()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = EnumUtils.Values<Suite>().ToDictionary(
						suite => suite,
						suite => suite == Suite.White ? BuildPile(suite, Number.Three) : BuildEmptyPile()),
				}.Build(),
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Score.Should().Be(3);
		}

		[Fact]
		public void ScoreReturnsSumOfTopCardValuesForMultiplePiles()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = new Dictionary<Suite, IPile>
					{
						[Suite.White] = BuildEmptyPile(),
						[Suite.Yellow] = BuildPile(Suite.Yellow, Number.One),
						[Suite.Green] = BuildPile(Suite.Green, Number.Two),
						[Suite.Blue] = BuildPile(Suite.Blue, Number.Three),
						[Suite.Red] = BuildFullPile(Suite.Red),
					},
				}.Build(),
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Score.Should().Be(0 + 1 + 2 + 3 + 5);
		}

		[Fact]
		public void ScoreReturnsMaxForAllFives()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = EnumUtils.Values<Suite>().ToDictionary(
						suite => suite,
						suite => BuildFullPile(suite)),
				}.Build(),
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Score.Should().Be(25);
		}
	}
}

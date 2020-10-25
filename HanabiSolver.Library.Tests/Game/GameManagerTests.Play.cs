using FluentAssertions;
using HanabiSolver.Common.Utils;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameManagerTests
	{
		[Fact]
		public void PlayWithoutTacticsThrowsException()
		{
			var gameState = new GameStateBuilder().Build();
			var gameManager = new GameManager(gameState);

			gameManager
				.Invoking(gm => gm.Play(Enumerable.Empty<ITactics>()))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		public void PlayRunsUntilGameIsEnded(int turnsUntilEnd)
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = BuildFinishingPlayedCards(turnsUntilEnd),
				}.Build(),
				Players = BuildSomePlayers(),
			}.Build();
			var gameManager = new GameManager(gameState);

			var tactics = BuildSomeTactics();
			gameManager.Play(tactics);

			gameManager.IsEnded.Should().BeTrue();
		}

		[Theory]
		[InlineData(2, 0, 0)]
		[InlineData(2, 1, 1)]
		[InlineData(2, 2, 0)]
		[InlineData(2, 3, 1)]
		[InlineData(2, 4, 0)]
		[InlineData(3, 0, 0)]
		[InlineData(3, 1, 1)]
		[InlineData(3, 2, 2)]
		[InlineData(3, 3, 0)]
		public void PlayCyclesCurrentPlayer(int playerCount, int turnsUntilEnd, int finishingPlayer)
		{
			var players = BuildSomePlayers(playerCount);
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = BuildFinishingPlayedCards(turnsUntilEnd),
				}.Build(),
				Players = players,
			}.Build();
			var gameManager = new GameManager(gameState);

			var tactics = BuildSomeTactics();
			gameManager.Play(tactics);

			gameState.CurrentPlayer.Should().Be(players[finishingPlayer]);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void PlayAppliesTacticsWithGameState(int turnsUntilEnd)
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = BuildFinishingPlayedCards(turnsUntilEnd),
				}.Build(),
				Players = BuildSomePlayers(3),
			}.Build();
			var gameManager = new GameManager(gameState);

			var tactics = new Mock<ITactics>(MockBehavior.Strict);
			tactics
				.Setup(t => t.CanApply(gameState))
				.Returns(true);
			tactics
				.Setup(t => t.Apply(gameState));

			var tacticsList = new List<ITactics> { tactics.Object };
			gameManager.Play(tacticsList);

			foreach (var turnIndex in Enumerable.Range(0, turnsUntilEnd))
				tactics.Verify(t => t.Apply(gameState));
		}

		[Fact]
		public void PlayAppliesFirstApplicableTactics()
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = BuildFinishingPlayedCards(1),
				}.Build(),
				Players = BuildSomePlayers(),
			}.Build();
			var gameManager = new GameManager(gameState);

			var nonApplicableTactics = new Mock<ITactics>(MockBehavior.Strict);
			nonApplicableTactics
				.Setup(t => t.CanApply(gameState))
				.Returns(false);
			var applicableTactics = new Mock<ITactics>(MockBehavior.Strict);
			applicableTactics
				.Setup(t => t.CanApply(gameState))
				.Returns(true);
			applicableTactics
				.Setup(t => t.Apply(gameState));
			var tacticsList = new List<ITactics>
			{
				nonApplicableTactics.Object,
				applicableTactics.Object,
			};
			gameManager.Play(tacticsList);

			applicableTactics.Verify(t => t.Apply(gameState), Times.Once);
		}

		[Theory]
		[InlineData(3)]
		public void PlayEndsGameAfterOneRoundWhenDeckBecomesEmpty(int playerCount)
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					Deck = BuildEmptyDeck(),
				}.Build(),
				Players = BuildSomePlayers(playerCount),
			}.Build();
			var gameManager = new GameManager(gameState);

			var tactics = new Mock<ITactics>(MockBehavior.Strict);
			tactics
				.Setup(t => t.CanApply(gameState))
				.Returns(true);
			tactics
				.Setup(t => t.Apply(gameState));
			var tacticsList = new List<ITactics>
			{
				tactics.Object,
			};
			gameManager.Play(tacticsList);

			tactics.Verify(t => t.Apply(gameState), Times.Exactly(playerCount));
		}

		private Dictionary<Suite, IPile> BuildFinishingPlayedCards(int turnsUntilFinish)
		{
			return EnumUtils
				.Values<Suite>()
				.ToDictionary(
					suite => suite,
					suite => suite == Suite.White
						? BuildFinishingPile(suite, turnsUntilFinish)
						: BuildFullPile(suite));
		}

		private IDeck BuildEmptyDeck()
		{
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card>());
			return deck.Object;
		}

		private List<IPlayer> BuildSomePlayers() => BuildSomePlayers(3);

		private List<IPlayer> BuildSomePlayers(int playerCount)
		{
			return Enumerable
				.Range(0, playerCount)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
		}

		private List<ITactics> BuildSomeTactics()
		{
			var tactics = new Mock<ITactics>(MockBehavior.Strict);
			tactics
				.Setup(t => t.CanApply(It.IsAny<IGameState>()))
				.Returns(true);
			tactics
				.Setup(t => t.Apply(It.IsAny<IGameState>()));
			return new List<ITactics>
			{
				tactics.Object,
			};
		}
	}
}

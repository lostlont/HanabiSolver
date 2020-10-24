using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using HanabiSolver.Library.Utils;
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
			var players = BuildSomePlayers();
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
		public void PlayAppliesTacticsForCurrentPlayer(int turnsUntilEnd)
		{
			var players = BuildSomePlayers(3);
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					PlayedCards = BuildFinishingPlayedCards(turnsUntilEnd),
				}.Build(),
				Players = players,
			}.Build();
			var gameManager = new GameManager(gameState);

			var tactics = new Mock<ITactics>(MockBehavior.Strict);
			var applySequence = new MockSequence();
			foreach (var turnIndex in Enumerable.Range(0, turnsUntilEnd))
				tactics
					.InSequence(applySequence)
					.Setup(p => p.Apply(players[turnIndex % players.Count]));

			gameManager.Play(new List<ITactics> { tactics.Object });

			foreach (var turnIndex in Enumerable.Range(0, turnsUntilEnd))
				tactics.Verify(t => t.Apply(players[turnIndex % players.Count]));
		}

		// TODO Test multiple tactics, and CanApply

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

		private IPile BuildFinishingPile(int turnsUntilFinish) => BuildFinishingPile(Suite.White, turnsUntilFinish);

		private IPile BuildFinishingPile(Suite suite, int turnsUntilFinish)
		{
			var playedCards = new Mock<IPile>(MockBehavior.Strict);
			var topSequence = playedCards.SetupSequence(p => p.Top);

			var card = new Card(suite, Number.Four);
			foreach (var turnIndex in Enumerable.Range(0, turnsUntilFinish))
				topSequence.Returns(card);

			var lastCard = new Card(suite, Number.Five);
			topSequence
				.Returns(lastCard)
				.Returns(lastCard);

			return playedCards.Object;
		}

		private IDeck BuildSomeDeck()
		{
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card> { new Card(Suite.White, Number.One) });
			return deck.Object;
		}

		private IDeck BuildEmptyDeck()
		{
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card>());
			return deck.Object;
		}

		private IPile BuildFullPile(Suite suite)
		{
			var pile = new Mock<IPile>(MockBehavior.Strict);
			pile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.Five));
			return pile.Object;
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
			tactics.Setup(t => t.Apply(It.IsAny<IPlayer>()));
			return new List<ITactics>
			{
				tactics.Object,
			};
		}
	}
}

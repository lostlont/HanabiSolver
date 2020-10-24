using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameManagerTests
	{
		[Fact]
		public void PlaySetsCurrentPlayerToNextOne()
		{
			var players = BuildSomePlayers();
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					Deck = BuildSomeDeck(),
				}.Build(),
				Players = players,
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Play();

			gameState.CurrentPlayer.Should().Be(players[1]);
		}

		[Fact]
		public void PlaySetsCurrentPlayerToFirstOneForLastOne()
		{
			var players = BuildSomePlayers();
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					Deck = BuildSomeDeck(),
				}.Build(),
				Players = players,
				CurrentPlayer = players.Last(),
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Play();

			gameState.CurrentPlayer.Should().Be(players.First());
		}

		// TODO Tests for simulating with non-finished states.

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

		private List<IPlayer> BuildSomePlayers() => BuildSomePlayers(3);

		private List<IPlayer> BuildSomePlayers(int playerCount)
		{
			return Enumerable
				.Range(0, playerCount)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
		}
	}
}

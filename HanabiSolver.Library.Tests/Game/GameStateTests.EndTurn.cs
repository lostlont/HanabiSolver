using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameStateTests
	{
		[Fact]
		public void EndTurnSetsCurrentPlayerToNextOne()
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

			gameState.EndTurn();

			gameState.CurrentPlayer.Should().Be(players[1]);
		}

		[Fact]
		public void EndTurnSetsCurrentPlayerToFirstOneForLastOne()
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

			gameState.EndTurn();

			gameState.CurrentPlayer.Should().Be(players.First());
		}

		[Theory]
		[InlineData(3, 2, false)]
		[InlineData(3, 3, true)]
		[InlineData(10, 9, false)]
		[InlineData(10, 10, true)]
		public void EndTurnSetsIsEndedForEmptiedDeckAfterOneRound(int playerCount, int turnCount, bool isEnded)
		{
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					Deck = BuildEmptyDeck(),
				}.Build(),
				Players = BuildSomePlayers(playerCount),
			}.Build();

			foreach (var turnIndex in Enumerable.Range(0, turnCount))
				gameState.EndTurn();

			gameState.IsEnded.Should().Be(isEnded);
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

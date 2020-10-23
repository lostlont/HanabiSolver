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
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card> { new Card(Suite.White, Number.One) });
			var tableBuilder = new TableBuilder
			{
				Deck = deck.Object,
			};
			var table = tableBuilder.Build();
			var players = Enumerable
				.Range(0, 3)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
			var gameState = new GameState(table, players);

			gameState.EndTurn();

			gameState.CurrentPlayer.Should().Be(players[1]);
		}

		[Fact]
		public void EndTurnSetsCurrentPlayerToFirstOneForLastOne()
		{
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card> { new Card(Suite.White, Number.One) });
			var tableBuilder = new TableBuilder
			{
				Deck = deck.Object,
			};
			var table = tableBuilder.Build();
			var players = Enumerable
				.Range(0, 3)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
			var gameState = new GameState(table, players, players.Last());

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
			var deck = new Mock<IDeck>(MockBehavior.Strict);
			deck
				.Setup(d => d.Cards)
				.Returns(new List<Card>());
			var tableBuilder = new TableBuilder
			{
				Deck = deck.Object,
			};
			var table = tableBuilder.Build();
			var players = Enumerable
				.Range(0, playerCount)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
			var gameState = new GameState(table, players);

			foreach (var turnIndex in Enumerable.Range(0, turnCount))
				gameState.EndTurn();

			gameState.IsEnded.Should().Be(isEnded);
		}
	}
}

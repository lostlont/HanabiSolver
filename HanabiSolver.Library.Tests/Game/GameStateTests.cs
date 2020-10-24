using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public class GameStateTests
	{
		private readonly Table someTable;
		private readonly IReadOnlyList<IPlayer> somePlayers;

		public GameStateTests()
		{
			someTable = new TableBuilder().Build();
			somePlayers = Enumerable
				.Range(0, 3)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
		}

		[Fact]
		public void ConstructorSetsPlayers()
		{
			var gameState = new GameState(someTable, somePlayers);

			gameState.Players.Should().Equal(somePlayers);
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToFirstPlayer()
		{
			var players = new GameState(someTable, somePlayers);

			players.CurrentPlayer.Should().Be(somePlayers.First());
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToGivenPlayer()
		{
			var players = new GameState(someTable, somePlayers, somePlayers[1]);

			players.CurrentPlayer.Should().Be(somePlayers[1]);
		}

		[Fact]
		public void NextPlayerIsTheSecondForCurrentPlayerAsFirst()
		{
			var players = new GameState(someTable, somePlayers);

			players.NextPlayer.Should().Be(somePlayers[1]);
		}

		[Fact]
		public void NextPlayerIsTheThirdForCurrentPlayerAsSecond()
		{
			var players = new GameState(someTable, somePlayers, somePlayers[1]);

			players.NextPlayer.Should().Be(somePlayers[2]);
		}

		[Fact]
		public void NextPlayerIsTheFirstForCurrentPlayerAsLast()
		{
			var players = new GameState(someTable, somePlayers, somePlayers.Last());

			players.NextPlayer.Should().Be(somePlayers.First());
		}
	}
}

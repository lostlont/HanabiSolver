using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameStateTests
	{
		// TODO Create a GameStateBuilder and get rid of the parts.
		private readonly Table table;
		private readonly IReadOnlyList<Player> playerList;

		public GameStateTests()
		{
			table = new TableBuilder().Build();
			playerList = new List<Player>
			{
				new Player(Enumerable.Empty<Card>(), table),
				new Player(Enumerable.Empty<Card>(), table),
				new Player(Enumerable.Empty<Card>(), table),
			};
		}

		[Fact]
		public void ConstructorSetsPlayers()
		{
			var gameState = new GameState(table, playerList);

			gameState.Players.Should().Equal(playerList);
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToFirstPlayer()
		{
			var players = new GameState(table, playerList);

			players.CurrentPlayer.Should().Be(playerList.First());
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToGivenPlayer()
		{
			var players = new GameState(table, playerList, playerList[1]);

			players.CurrentPlayer.Should().Be(playerList[1]);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameStateTests
	{
		[Fact]
		public void EndTurnSetsCurrentPlayerToNextOne()
		{
			var table = new TableBuilder().Build();
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
			var table = new TableBuilder().Build();
			var players = Enumerable
				.Range(0, 3)
				.Select(_ => new Mock<IPlayer>().Object)
				.ToList();
			var gameState = new GameState(table, players, players.Last());

			gameState.EndTurn();

			gameState.CurrentPlayer.Should().Be(players.First());
		}
	}
}

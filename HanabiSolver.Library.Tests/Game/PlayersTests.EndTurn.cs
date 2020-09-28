using FluentAssertions;
using HanabiSolver.Library.Game;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayersTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		public void EndTurnSetsCurrentPlayerToNextInLine(int index)
		{
			var currentPlayer = playerList[index];
			var players = new Players(playerList, currentPlayer);
			var nextPlayer = players.Next(currentPlayer);

			players.EndTurn();

			players.CurrentPlayer.Should().Be(nextPlayer);
		}
	}
}

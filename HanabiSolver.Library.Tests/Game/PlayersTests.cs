using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayersTests
	{
		private readonly IReadOnlyList<Player> playerList;

		public PlayersTests()
		{
			var table = new TableBuilder().Build();
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
			var players = new Players(playerList);

			players.Should().Equal(playerList);
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToFirstPlayer()
		{
			var players = new Players(playerList);

			players.CurrentPlayer.Should().Be(playerList.First());
		}

		[Fact]
		public void ConstructorSetsCurrentPlayerToGivenPlayer()
		{
			var players = new Players(playerList, playerList[1]);

			players.CurrentPlayer.Should().Be(playerList[1]);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public class PlayersTests
	{
		[Fact]
		public void ConstructorSetsPlayers()
		{
			var table = new TableBuilder().Build();
			var threePlayerList = new List<Player>
			{
				new Player(Enumerable.Empty<Card>(), table),
				new Player(Enumerable.Empty<Card>(), table),
				new Player(Enumerable.Empty<Card>(), table),
			};
			var players = new Players(threePlayerList);

			players.Should().Equal(threePlayerList);
		}
	}
}

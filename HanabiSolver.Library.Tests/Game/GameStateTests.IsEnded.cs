using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameStateTests
	{
		[Fact]
		public void IsNotEndedNormally()
		{
			var table = new TableBuilder().Build();
			var players = new List<Player>
			{
				new Player(new Card(Suite.White, Number.One).AsEnumerable(), table),
				new Player(new Card(Suite.Yellow, Number.Two).AsEnumerable(), table),
				new Player(new Card(Suite.Green, Number.Three).AsEnumerable(), table),
			};
			var gameState = new GameState(table, players);

			gameState.IsEnded.Should().BeFalse();
		}
	}
}

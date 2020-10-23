using HanabiSolver.Library.Game;
using Moq;
using System.Collections.Generic;

namespace HanabiSolver.Library.Tests.Builders
{
	public class GameStateBuilder
	{
		public Table Table { get; set; } = new TableBuilder().Build();
		public IReadOnlyList<IPlayer> Players { get; set; } = BuildSomePlayers();
		public IPlayer? CurrentPlayer { get; set; } = null;

		public GameState Build()
		{
			if (CurrentPlayer == null)
				return new GameState(Table, Players);
			else
				return new GameState(Table, Players, CurrentPlayer);
		}

		private static List<IPlayer> BuildSomePlayers()
		{
			return new List<IPlayer>
			{
				new Mock<IPlayer>(MockBehavior.Strict).Object,
				new Mock<IPlayer>(MockBehavior.Strict).Object,
				new Mock<IPlayer>(MockBehavior.Strict).Object,
			};
		}
	}
}

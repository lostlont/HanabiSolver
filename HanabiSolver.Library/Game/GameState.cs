using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class GameState
	{
		public GameState(Table table, IEnumerable<Player> players)
			: this(table, players, players.First())
		{
		}

		public GameState(Table table, IEnumerable<Player> players, Player currentPlayer)
		{
			Table = table;
			Players = players.ToList();
			CurrentPlayer = currentPlayer;
		}

		public Table Table { get; }
		public IReadOnlyList<Player> Players { get; }
		public Player CurrentPlayer { get; }
		public bool IsEnded { get; }
	}
}

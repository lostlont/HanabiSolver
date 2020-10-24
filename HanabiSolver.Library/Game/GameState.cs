using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IGameState
	{
		Table Table { get; }
		IReadOnlyList<IPlayer> Players { get; }
		IPlayer CurrentPlayer { get; set; }
	}

	public class GameState : IGameState
	{
		public GameState(Table table, IReadOnlyList<IPlayer> players)
			: this(table, players, players.First())
		{
		}

		public GameState(Table table, IEnumerable<IPlayer> players, IPlayer currentPlayer)
		{
			Table = table;
			Players = players.ToList();
			CurrentPlayer = currentPlayer;
		}

		public Table Table { get; }
		public IReadOnlyList<IPlayer> Players { get; }
		public IPlayer CurrentPlayer { get; set; }
	}
}

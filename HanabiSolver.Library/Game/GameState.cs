using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class GameState
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
		public IPlayer NextPlayer
		{
			get
			{
				var nextPlayer = Players
					.SkipWhile(player => player != CurrentPlayer)
					.Skip(1)
					.FirstOrDefault();
				return nextPlayer ?? Players.First();
			}
		}
	}
}

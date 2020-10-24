using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IReadOnlyGameState
	{
		IReadOnlyTable Table { get; }
		IReadOnlyList<IPlayer> Players { get; }
		IPlayer CurrentPlayer { get; }
		IPlayer NextPlayer { get; }
	}

	public class GameState : IReadOnlyGameState
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

		IReadOnlyTable IReadOnlyGameState.Table => Table;
	}
}

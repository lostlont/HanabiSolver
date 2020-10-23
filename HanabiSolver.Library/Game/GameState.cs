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
		public IPlayer CurrentPlayer { get; }
		public bool IsEnded
		{
			get
			{
				return
					(Table.FuseTokens.Amount >= Table.FuseTokens.MaxAmount) ||
					(Table.PlayedCards.Values.All(pile => pile.Top?.Number == Number.Five));
			}
		}
	}
}

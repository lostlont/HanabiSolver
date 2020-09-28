using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Players : IReadOnlyList<Player>
	{
		private readonly List<Player> players;

		public Players(IEnumerable<Player> players)
		{
			this.players = players.ToList();
		}

		public Player this[int index] => players[index];
		public int Count => players.Count;

		public IEnumerator<Player> GetEnumerator() => players.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();
	}
}

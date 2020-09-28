﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Players : IReadOnlyList<Player>
	{
		private readonly List<Player> players;

		public Players(IEnumerable<Player> players)
			: this(players, players.First())
		{
		}

		public Players(IEnumerable<Player> players, Player currentPlayer)
		{
			this.players = players.ToList();
			CurrentPlayer = currentPlayer;
		}

		public Player CurrentPlayer { get; private set; }
		public Player this[int index] => players[index];
		public int Count => players.Count;

		public IEnumerator<Player> GetEnumerator() => players.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();

		public Player Next(Player player)
		{
			return players
				.Append(players.First())
				.SkipWhile(p => p != player)
				.Skip(1)
				.First();
		}

		public void EndTurn()
		{
			CurrentPlayer = Next(CurrentPlayer);
		}
	}
}

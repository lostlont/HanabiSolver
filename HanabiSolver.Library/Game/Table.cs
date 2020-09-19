using HanabiSolver.Library.Interfaces;
using HanabiSolver.Library.Utils;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Table : ITable
	{
		public Deck Deck { get; }
		public Pile DiscardPile { get; }
		public Tokens Tokens { get; }
		public Dictionary<Suite, Pile> PlayedCards { get; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Pile());

		public Table(Deck deck, Pile discardPile, Tokens tokens)
		{
			Deck = deck;
			DiscardPile = discardPile;
			Tokens = tokens;
		}
	}
}

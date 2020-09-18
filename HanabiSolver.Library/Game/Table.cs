using HanabiSolver.Library.Interfaces;

namespace HanabiSolver.Library.Game
{
	public class Table : ITable
	{
		public Deck Deck { get; }
		public Pile DiscardPile { get; }
		public Tokens Tokens { get; }

		public Table(Deck deck, Pile discardPile, Tokens tokens)
		{
			Deck = deck;
			DiscardPile = discardPile;
			Tokens = tokens;
		}
	}
}

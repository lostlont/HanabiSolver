using HanabiSolver.Library.Interfaces;

namespace HanabiSolver.Library.Game
{
	public class Table : ITable
	{
		public Deck Deck { get; }
		public Pile DiscardPile { get; }

		public Table(Deck deck, Pile discardPile)
		{
			Deck = deck;
			DiscardPile = discardPile;
		}
	}
}

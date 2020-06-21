namespace HanabiSolver.Library.Game
{
	public class Table
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

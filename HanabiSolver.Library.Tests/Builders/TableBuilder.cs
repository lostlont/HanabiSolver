using HanabiSolver.Library.Game;
using HanabiSolver.Library.Interfaces;
using System;

namespace HanabiSolver.Library.Tests.Builders
{
	public class TableBuilder
	{
		public Func<Deck> DeckBuilder { get; set; }
		public Func<Pile> DiscardPileBuilder { get; set; }

		public TableBuilder(Func<Deck> deckBuilder, Func<Pile> discardPileBuilder)
		{
			DeckBuilder = deckBuilder;
			DiscardPileBuilder = discardPileBuilder;
		}

		public ITable Build()
		{
			var deck = DeckBuilder();
			var discardPile = DiscardPileBuilder();
			return new Table(deck, discardPile);
		}
	}
}

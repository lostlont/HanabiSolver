using HanabiSolver.Library.Game;
using HanabiSolver.Library.Interfaces;
using System;

namespace HanabiSolver.Library.Tests.Builders
{
	public class TableBuilder
	{
		public Func<Deck> DeckBuilder { get; set; }
		public Func<Pile> DiscardPileBuilder { get; set; }
		public Func<Tokens> TokensBuilder { get; }

		public TableBuilder(Func<Deck> deckBuilder, Func<Pile> discardPileBuilder, Func<Tokens> tokensBuilder)
		{
			DeckBuilder = deckBuilder;
			DiscardPileBuilder = discardPileBuilder;
			TokensBuilder = tokensBuilder;
		}

		public ITable Build()
		{
			var deck = DeckBuilder();
			var discardPile = DiscardPileBuilder();
			var tokens = TokensBuilder();
			return new Table(deck, discardPile, tokens);
		}
	}
}

using HanabiSolver.Library.Game;
using HanabiSolver.Library.Interfaces;
using HanabiSolver.Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class TableBuilder
	{
		private static Func<Pile> DefaultPileBuilder { get; } = () => new Pile();

		public Func<Deck> DeckBuilder { get; set; } = () => new Deck(Enumerable.Empty<Card>());
		public Func<Pile> DiscardPileBuilder { get; set; } = () => new Pile();
		public Func<Tokens> TokensBuilder { get; set; } = () => new Tokens(3);
		public Dictionary<Suite, Func<Pile>> PlayedCardsBuilder { get; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => DefaultPileBuilder);

		public ITable Build()
		{
			var deck = DeckBuilder();
			var discardPile = DiscardPileBuilder();
			var tokens = TokensBuilder();
			var playedCards = PlayedCardsBuilder.Keys.ToDictionary(suite => suite, suite => PlayedCardsBuilder[suite]());
			return new Table(deck, discardPile, tokens, playedCards);
		}

		// TODO Change all the Builders to have property-setters?
	}
}

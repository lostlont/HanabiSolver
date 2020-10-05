using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class TableBuilder
	{
		private static Func<IPile> DefaultPileBuilder { get; } = () => new Pile();

		public IDeck Deck { get; set; }
		public IPile DiscardPile { get; set; }
		public ITokens InformationTokens { get; set; }
		public Func<Tokens> FuseTokensBuilder { get; set; } = () => new Tokens(2, 0);
		public Dictionary<Suite, Func<IPile>> PlayedCardsBuilder { get; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => DefaultPileBuilder);

		public TableBuilder()
		{
			var someDeck = new Mock<IDeck>();
			someDeck
				.SetupSequence(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One))
				.Returns(new Card(Suite.Green, Number.Two))
				.Returns(new Card(Suite.Blue, Number.Three));
			Deck = someDeck.Object;
			DiscardPile = new Mock<IPile>().Object;
			InformationTokens = new Mock<ITokens>().Object;
		}

		public Table Build()
		{
			var fuseTokens = FuseTokensBuilder();
			var playedCards = PlayedCardsBuilder.Keys.ToDictionary(suite => suite, suite => PlayedCardsBuilder[suite]());
			return new Table(Deck, DiscardPile, InformationTokens, fuseTokens, playedCards);
		}
	}
}

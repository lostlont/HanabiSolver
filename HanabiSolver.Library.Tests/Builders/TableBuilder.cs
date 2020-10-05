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

		public IDeck Deck { get; set; } = BuildSomeDeck();
		public IPile DiscardPile { get; set; } = new Mock<IPile>().Object;
		public ITokens InformationTokens { get; set; } = BuildSomeTokens();
		public Func<Tokens> FuseTokensBuilder { get; set; } = () => new Tokens(2, 0);
		public Dictionary<Suite, Func<IPile>> PlayedCardsBuilder { get; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => DefaultPileBuilder);

		public Table Build()
		{
			var fuseTokens = FuseTokensBuilder();
			var playedCards = PlayedCardsBuilder.Keys.ToDictionary(suite => suite, suite => PlayedCardsBuilder[suite]());
			return new Table(Deck, DiscardPile, InformationTokens, fuseTokens, playedCards);
		}

		public static IDeck BuildSomeDeck()
		{
			var deck = new Mock<IDeck>();
			deck
				.SetupSequence(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One))
				.Returns(new Card(Suite.Green, Number.Two))
				.Returns(new Card(Suite.Blue, Number.Three));

			return deck.Object;
		}

		public static ITokens BuildSomeTokens()
		{
			var tokens = new Mock<ITokens>();
			tokens
				.Setup(t => t.Amount)
				.Returns(1);

			return tokens.Object;
		}

		public static ITokens BuildEmptyTokens()
		{
			var tokens = new Mock<ITokens>();
			tokens
				.Setup(t => t.Amount)
				.Returns(0);

			return tokens.Object;
		}
	}
}

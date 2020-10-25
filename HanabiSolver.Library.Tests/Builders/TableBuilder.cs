using HanabiSolver.Common.Utils;
using HanabiSolver.Library.Game;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class TableBuilder
	{
		public IDeck Deck { get; set; } = BuildSomeDeck();
		public IPile DiscardPile { get; set; } = new Mock<IPile>().Object;
		public ITokens InformationTokens { get; set; } = BuildSomeTokens();
		public ITokens FuseTokens { get; set; } = BuildEmptyTokens();
		public Dictionary<Suite, IPile> PlayedCards { get; set; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => BuildEmptyPile());

		public Table Build()
		{
			return new Table(Deck, DiscardPile, InformationTokens, FuseTokens, PlayedCards);
		}

		private static IDeck BuildSomeDeck()
		{
			var cards = new List<Card>
			{
				new Card(Suite.Red, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Blue, Number.Three),
			};
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Cards)
				.Returns(cards);
			deck
				.Setup(d => d.Draw())
				.Returns(cards.First());

			return deck.Object;
		}

		private static ITokens BuildSomeTokens()
		{
			var tokens = new Mock<ITokens>();
			tokens
				.Setup(t => t.Amount)
				.Returns(1);

			return tokens.Object;
		}

		private static ITokens BuildEmptyTokens()
		{
			var tokens = new Mock<ITokens>();
			tokens
				.Setup(t => t.MaxAmount)
				.Returns(3);
			tokens
				.Setup(t => t.Amount)
				.Returns(0);

			return tokens.Object;
		}

		private static IPile BuildEmptyPile()
		{
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Top)
				.Returns((Card?)null);

			return pile.Object;
		}
	}
}

using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
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
			var deck = new Mock<IDeck>();
			deck
				.SetupSequence(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One))
				.Returns(new Card(Suite.Green, Number.Two))
				.Returns(new Card(Suite.Blue, Number.Three));

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

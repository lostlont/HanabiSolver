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
		public Dictionary<Suite, IPile> PlayedCards { get; } = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => BuildEmptyPile());

		public Table Build()
		{
			return new Table(Deck, DiscardPile, InformationTokens, FuseTokens, PlayedCards);
		}

		// TODO Privatize?
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

		public static IPile BuildEmptyPile()
		{
			// TODO Use Top in Player.CanPlay and get rid of this!
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Cards)
				.Returns(new List<Card>());

			return pile.Object;
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public class PileTests
	{
		private readonly IReadOnlyList<Card> cards;

		public PileTests()
		{
			cards = new List<Card>
			{
				new Card(Suite.Blue, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Red, Number.Three),
			};
		}

		[Fact]
		public void CardsReturnsAllAddedCards()
		{
			var pile = new Pile(cards);

			pile.Cards.Should().Equal(cards);
		}

		[Fact]
		public void TopReturnsLastAddedCard()
		{
			var pile = new Pile(cards);

			pile.Top.Should().Be(cards.Last());
		}

		[Fact]
		public void TopReturnsNullOnEmpty()
		{
			var pile = new Pile();

			pile.Top.Should().BeNull();
		}
	}
}

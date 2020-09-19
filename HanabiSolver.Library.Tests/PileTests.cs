using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public class PileTests
	{
		private readonly IReadOnlyList<Card> cards;
		private readonly PileBuilder pileBuilder;

		public PileTests()
		{
			cards = new List<Card>
			{
				new Card(Suite.Blue, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Red, Number.Three),
			};
			pileBuilder = new PileBuilder(cards);
		}

		[Fact]
		public void CardsReturnsAllAddedCards()
		{
			var pile = pileBuilder.Build();

			pile.Cards.Should().BeEquivalentTo(cards);
		}

		[Fact]
		public void TopReturnsLastAddedCard()
		{
			var pile = pileBuilder.Build();

			pile.Top.Should().Be(cards.Last());
		}

		[Fact]
		public void TopReturnsNullOnEmpty()
		{
			pileBuilder.Cards = new List<Card>();
			var pile = pileBuilder.Build();

			pile.Top.Should().BeNull();
		}
	}
}

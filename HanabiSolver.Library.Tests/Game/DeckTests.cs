using FluentAssertions;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public class DeckTests
	{
		private readonly List<Card> cards;
		private readonly Deck deck;

		public DeckTests()
		{
			cards = new List<Card>
			{
				new Card(Suite.Red, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Blue, Number.Three),
			};
			deck = new Deck(cards);
		}

		[Fact]
		public void DrawReturnsFirst()
		{
			var drawnCard = deck.Draw();

			drawnCard.Should().Be(cards[0]);
		}

		[Fact]
		public void DrawLeavesAllButFirst()
		{
			deck.Draw();

			var expectedCards = cards.Skip(1);
			deck.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void DrawTwiceReturnsSecond()
		{
			deck.Draw();
			var secondDrawnCard = deck.Draw();

			secondDrawnCard.Should().Be(cards[1]);
		}

		[Fact]
		public void DrawReturnsNoneForEmpty()
		{
			var drawnCard = Deck.Empty.Draw();

			drawnCard.Should().BeNull();
		}
	}
}

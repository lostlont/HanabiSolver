using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using System;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public partial class PlayerTests
	{
		[Fact]
		public void DiscardAddsCardToDiscardPile()
		{
			var player = playerBuilder.Build();

			player.Discard(cardsInHand[0]);

			var expectedCards = cardsInHand.Take(1);
			player.Table.DiscardPile.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void DiscardingUnownedCardThrowsException()
		{
			var player = playerBuilder.Build();

			var unownedCard = new Card(Suite.Blue, Number.Five);
			player
				.Invoking(player => player.Discard(unownedCard))
				.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void DiscardDrawsFromDeck()
		{
			var player = playerBuilder.Build();

			player.Discard(cardsInHand[0]);

			var expectedDeck = new Deck(cardsInDeck);
			expectedDeck.Draw();

			player.Table.Deck.Cards.Should().Equal(expectedDeck.Cards);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void DiscardMovesFromTopOfDeckToBeginningOfHand(int cardIndexToDiscard)
		{
			var player = playerBuilder.Build();
			var newCard = player.Table.Deck.Top;

			player.Discard(cardsInHand[cardIndexToDiscard]);

			var newCards = newCard.AsEnumerable();
			var oldCards = cardsInHand.ExceptAt(cardIndexToDiscard);
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void DiscardReplenishesInformationToken()
		{
			var player = playerBuilder.Build();

			player.Discard(cardsInHand[0]);

			player.Table.InformationTokens.Amount.Should().Be(2);
		}
	}
}

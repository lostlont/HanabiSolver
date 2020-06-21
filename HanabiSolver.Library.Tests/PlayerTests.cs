using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public class PlayerTests
	{
		private readonly List<Card> cardsInHand;
		private readonly List<Card> cardsInDeck;
		private readonly Deck deck;
		private readonly List<Card> discardPile;
		private readonly Player player;

		public PlayerTests()
		{
			cardsInHand = new List<Card>
			{
				new Card(Suite.White, Number.Five),
				new Card(Suite.White, Number.One),
				new Card(Suite.Red, Number.Three),
			};

			cardsInDeck = new List<Card>
			{
				new Card(Suite.Red, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Blue, Number.Three),
			};
			deck = new Deck(cardsInDeck);

			discardPile = new List<Card>();

			player = new Player(cardsInHand, deck, discardPile);
		}

		[Fact]
		public void DiscardAddsCardToDiscardPile()
		{
			player.Discard(cardsInHand[0]);

			var expectedCards = new List<Card> { cardsInHand[0] };
			discardPile.Should().Equal(expectedCards);
		}

		[Fact]
		public void DiscardingUnownedCardThrowsException()
		{
			var unownedCard = new Card(Suite.Blue, Number.Five);
			player
				.Invoking(player => player.Discard(unownedCard))
				.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void DiscardDrawsFromDeck()
		{
			player.Discard(cardsInHand[0]);

			var expectedDeck = new Deck(cardsInDeck);
			expectedDeck.Draw();

			deck.Cards.Should().Equal(expectedDeck.Cards);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void DiscardMovesFromTopOfDeckToBeginningOfHand(int cardIndexToDiscard)
		{
			var newCard = deck.Top;

			player.Discard(cardsInHand[cardIndexToDiscard]);

			var newCards = newCard.AsEnumerable();
			var oldCards = cardsInHand.ExceptAt(cardIndexToDiscard);
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}


		// TODO Discarding should add token.
		// TODO Test discarding when tokens are full?
	}
}

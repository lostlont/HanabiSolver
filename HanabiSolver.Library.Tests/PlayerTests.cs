using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Interfaces;
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
		private readonly ITable table;
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

			var deck = new Deck(cardsInDeck);
			var discardPile = new Pile();
			table = new Table(deck, discardPile);

			player = new Player(cardsInHand, table);
		}

		[Fact]
		public void DiscardAddsCardToDiscardPile()
		{
			player.Discard(cardsInHand[0]);

			var expectedCards = cardsInHand.Take(1);
			table.DiscardPile.Cards.Should().Equal(expectedCards);
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

			table.Deck.Cards.Should().Equal(expectedDeck.Cards);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void DiscardMovesFromTopOfDeckToBeginningOfHand(int cardIndexToDiscard)
		{
			var newCard = table.Deck.Top;

			player.Discard(cardsInHand[cardIndexToDiscard]);

			var newCards = newCard.AsEnumerable();
			var oldCards = cardsInHand.ExceptAt(cardIndexToDiscard);
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		// TODO Test tokens.
		// TODO Discarding should add token.
		// TODO Test discarding when tokens are full?
	}
}

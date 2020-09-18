using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public class PlayerTests
	{
		private readonly IReadOnlyList<Card> cardsInHand;
		private readonly IReadOnlyList<Card> cardsInDeck;
		private readonly PlayerBuilder playerBuilder;

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
			var tableBuilder = new TableBuilder(() => deck, () => discardPile);

			playerBuilder = new PlayerBuilder(cardsInHand, tableBuilder);
		}

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

		// TODO Test tokens.
		// TODO Discarding should add token.
		// TODO Test discarding when tokens are full?
	}
}

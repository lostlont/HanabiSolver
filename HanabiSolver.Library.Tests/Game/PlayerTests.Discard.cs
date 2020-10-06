using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void DiscardAddsCardToDiscardPile()
		{
			var discardPile = new Mock<IPile>();
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					DiscardPile = discardPile.Object,
				},
			};
			var player = playerBuilder.Build();

			player.Discard(player.Cards.First());

			discardPile.Verify(p => p.Add(It.IsAny<Card>()), Times.Once);
		}

		[Fact]
		public void DiscardingUnownedCardThrowsException()
		{
			var player = new PlayerBuilder().Build();
			var unownedCard = new Card(Suite.Blue, Number.Five);

			player
				.Invoking(player => player.Discard(unownedCard))
				.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void DiscardDrawsFromDeck()
		{
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One));

			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					Deck = deck.Object,
				},
			};
			var player = playerBuilder.Build();

			player.Discard(player.Cards.First());

			deck.Verify(d => d.Draw(), Times.Once);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void DiscardMovesFromDeckToBeginningOfHand(int cardIndexToDiscard)
		{
			var newCard = new Card(Suite.Red, Number.One);
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(newCard);

			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					Deck = deck.Object,
				},
			};
			var player = playerBuilder.Build();

			var oldCards = player.Cards
				.ExceptAt(cardIndexToDiscard)
				.ToList();

			player.Discard(player.Cards[cardIndexToDiscard]);

			var newCards = newCard.AsEnumerable();
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void DiscardReplenishesInformationToken()
		{
			var informationTokens = new Mock<ITokens>();
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();

			player.Discard(player.Cards.First());

			informationTokens.Verify(t => t.Replenish(), Times.Once);
		}

		[Fact]
		public void DiscardGetsRidOfInformation()
		{
			var player = new PlayerBuilder().Build();

			var cardToDiscard = player.Cards.First();
			player.Discard(cardToDiscard);

			player.Information.Keys.Should().NotContain(cardToDiscard);
		}

		[Fact]
		public void DiscardAddsEmptyInformationForDrawnCard()
		{
			var newCard = new Card(Suite.Red, Number.One);
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(newCard);

			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					Deck = deck.Object,
				},
			};
			var player = playerBuilder.Build();

			player.Discard(player.Cards.First());

			player.Information.Keys.Should().Contain(newCard);
		}
	}
}

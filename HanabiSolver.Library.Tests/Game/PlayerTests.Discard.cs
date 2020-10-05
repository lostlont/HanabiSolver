using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		private readonly IReadOnlyList<Card> someCards = new List<Card>
		{
			new Card(Suite.White, Number.Five),
			new Card(Suite.White, Number.One),
			new Card(Suite.Red, Number.Three),
		};

		private readonly Mock<IDeck> someDeck = new Mock<IDeck>();

		private readonly Mock<IPile> emptyDiscardPile = new Mock<IPile>();

		private void Setup()
		{
			someDeck
				.SetupSequence(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One))
				.Returns(new Card(Suite.Green, Number.Two))
				.Returns(new Card(Suite.Blue, Number.Three));
		}

		[Fact]
		public void DiscardAddsCardToDiscardPile()
		{
			Setup();
			var table = new Table(
				someDeck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			player.Discard(player.Cards.First());

			emptyDiscardPile.Verify(p => p.Add(It.IsAny<Card>()), Times.Once);
		}

		[Fact]
		public void DiscardingUnownedCardThrowsException()
		{
			Setup();
			var table = new Table(
				someDeck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			var unownedCard = new Card(Suite.Blue, Number.Five);
			player
				.Invoking(player => player.Discard(unownedCard))
				.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void DiscardDrawsFromDeck()
		{
			Setup();
			var table = new Table(
				someDeck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			player.Discard(player.Cards.First());

			someDeck.Verify(d => d.Draw(), Times.Once);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void DiscardMovesFromDeckToBeginningOfHand(int cardIndexToDiscard)
		{
			Setup();
			var newCard = new Card(Suite.Red, Number.One);
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(newCard);
			var table = new Table(
				deck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			// TODO player.Cards should be indexable!
			player.Discard(player.Cards.ElementAt(cardIndexToDiscard));

			var newCards = newCard.AsEnumerable();
			var oldCards = someCards.ExceptAt(cardIndexToDiscard);
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void DiscardReplenishesInformationToken()
		{
			Setup();
			var informationTokens = new Mock<ITokens>();
			var table = new Table(
				someDeck.Object,
				emptyDiscardPile.Object,
				informationTokens.Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			player.Discard(player.Cards.First());

			informationTokens.Verify(t => t.Replenish(), Times.Once);
		}

		[Fact]
		public void DiscardGetsRidOfInformation()
		{
			Setup();
			var table = new Table(
				someDeck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			var cardToDiscard = player.Cards.First();
			player.Discard(cardToDiscard);

			player.Information.Keys.Should().NotContain(cardToDiscard);
		}

		[Fact]
		public void DiscardAddsEmptyInformationForDrawnCard()
		{
			Setup();
			var newCard = new Card(Suite.Red, Number.One);
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(newCard);
			var table = new Table(
				deck.Object,
				emptyDiscardPile.Object,
				new Mock<ITokens>().Object,
				new Mock<ITokens>().Object,
				EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => new Mock<IPile>().Object));
			var player = new Player(someCards, table);

			player.Discard(player.Cards.First());

			player.Information.Keys.Should().Contain(newCard);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void PlayOneAddsToCorrespondingEmptySuite()
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, Number.One);

			var playedPile = new Mock<IPile>();
			playedPile
				.Setup(p => p.Top)
				.Returns((Card?)null);

			var playerBuilder = new PlayerBuilder
			{
				Cards = cardToPlay.AsEnumerable(),
				TableBuilder = new TableBuilder
				{
					PlayedCards = new Dictionary<Suite, IPile>
					{
						[suite] = playedPile.Object,
					},
				},
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			playedPile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Fact]
		public void PlayNextAddsToCorrespondingSuiteWithFollowingCardOnTop()
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, Number.Two);

			var playedPile = new Mock<IPile>();
			playedPile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.One));

			var playerBuilder = new PlayerBuilder
			{
				Cards = cardToPlay.AsEnumerable(),
				TableBuilder = new TableBuilder
				{
					PlayedCards = new Dictionary<Suite, IPile>
					{
						[suite] = playedPile.Object,
					},
				},
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			playedPile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Theory]
		[InlineData(Number.One, Number.One)]
		[InlineData(Number.Two, null)]
		[InlineData(Number.Five, Number.Three)]
		public void PlayWrongNumberGivesFuseTokenAndDiscardsCard(Number playedNumber, Number? lastNumber)
		{
			// TODO Split complex tests!
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, playedNumber);
			var discardPile = new Mock<IPile>();
			var fuseTokens = new Mock<ITokens>();
			var topCardPlayed = lastNumber.HasValue
				? new Card(suite, lastNumber.Value)
				: null;
			var playedPile = new Mock<IPile>();
			playedPile
				.Setup(p => p.Top)
				.Returns(topCardPlayed);

			var playerBuilder = new PlayerBuilder
			{
				Cards = cardToPlay.AsEnumerable(),
				TableBuilder = new TableBuilder
				{
					DiscardPile = discardPile.Object,
					FuseTokens = fuseTokens.Object,
					PlayedCards = new Dictionary<Suite, IPile>
					{
						[suite] = playedPile.Object,
					},
				},
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			playedPile.Verify(p => p.Add(It.IsAny<Card>()), Times.Never);
			fuseTokens.Verify(t => t.Replenish(), Times.Once);
			discardPile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Fact]
		public void PlayFiveReplenishesInformationToken()
		{
			const Suite suite = Suite.Blue;
			var informationTokens = new Mock<ITokens>();
			var playedPile = new Mock<IPile>();
			playedPile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.Four));

			var playerBuilder = new PlayerBuilder
			{
				Cards = new Card(suite, Number.Five).AsEnumerable(),
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
					PlayedCards = new Dictionary<Suite, IPile>
					{
						[suite] = playedPile.Object,
					},
				},
			};
			var player = playerBuilder.Build();

			player.Play(player.Cards.First());

			informationTokens.Verify(t => t.Replenish(), Times.Once);
		}

		[Fact]
		public void PlayDrawsFromDeck()
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

			player.Play(player.Cards.First());

			deck.Verify(d => d.Draw(), Times.Once);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void PlayMovesFromTopOfDeckToBeginningOfHand(int cardIndexToPlay)
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
				.ExceptAt(cardIndexToPlay)
				.ToList();

			player.Play(player.Cards[cardIndexToPlay]);

			var newCards = newCard.AsEnumerable();
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void PlayGetsRidOfInformation()
		{
			var player = new PlayerBuilder().Build();
			var cardToPlay = player.Cards.First();

			player.Play(cardToPlay);

			player.Information.Keys.Should().NotContain(cardToPlay);
		}

		[Fact]
		public void PlayAddsEmptyInformationForDrawnCard()
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

			player.Play(player.Cards.First());

			player.Information.Keys.Should().Contain(newCard);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
using Moq;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void PlayOneDropsToTopOfCorrespondingEmptySuite()
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, Number.One);

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[suite].Top.Should().Be(cardToPlay);
		}

		[Fact]
		public void PlayNextDropsToTopOfCorrespondingSuiteWithFollowingCardOnTop()
		{
			const Suite suite = Suite.Blue;
			var cardsPlayed = new Card(suite, Number.One).AsEnumerable();
			var cardToPlay = new Card(suite, Number.Two);

			playerBuilder.TableBuilder.PlayedCardsBuilder[suite] = () => new Pile(cardsPlayed);
			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			var expectedCards = cardsPlayed.Append(cardToPlay);
			player.Table.PlayedCards[suite].Cards.Should().Equal(expectedCards);
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
			var cardsPlayed = lastNumber
				.ExistingAsEnumerable()
				.Select(n => new Card(suite, n))
				.ToList();

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var fuseTokens = new Mock<ITokens>();
			playerBuilder.TableBuilder.FuseTokens = fuseTokens.Object;
			playerBuilder.TableBuilder.PlayedCardsBuilder[suite] = () => new Pile(cardsPlayed);
			var discardPile = new Mock<IPile>();
			playerBuilder.TableBuilder.DiscardPile = discardPile.Object;
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[suite].Cards.Should().Equal(cardsPlayed);
			fuseTokens.Verify(t => t.Replenish(), Times.Once);
			discardPile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Fact]
		public void PlayFiveReplenishesInformationToken()
		{
			const Suite suite = Suite.Blue;
			var cardsPlayed = EnumUtils
				.Values<Number>()
				.SkipLast(1)
				.Select(n => new Card(suite, n));

			playerBuilder.Cards = new Card(suite, Number.Five).AsEnumerable();
			var informationTokens = new Mock<ITokens>();
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			playerBuilder.TableBuilder.PlayedCardsBuilder[suite] = () => new Pile(cardsPlayed);
			var player = playerBuilder.Build();

			player.Play(player.Cards.Single());

			informationTokens.Verify(t => t.Replenish(), Times.Once);
		}

		[Fact]
		public void PlayDrawsFromDeck()
		{
			var deck = new Mock<IDeck>();
			deck
				.Setup(d => d.Draw())
				.Returns(new Card(Suite.Red, Number.One));
			playerBuilder.TableBuilder.Deck = deck.Object;
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
			playerBuilder.TableBuilder.Deck = deck.Object;
			var player = playerBuilder.Build();

			var oldCards = player.Cards
				.ExceptAt(cardIndexToPlay)
				.ToList();

			// TODO player.Cards should be indexable!
			player.Play(player.Cards.ElementAt(cardIndexToPlay));

			var newCards = newCard.AsEnumerable();
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}

		[Fact]
		public void PlayGetsRidOfInformation()
		{
			var player = playerBuilder.Build();
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
			playerBuilder.TableBuilder.Deck = deck.Object;
			var player = playerBuilder.Build();

			player.Play(player.Cards.First());

			player.Information.Keys.Should().Contain(newCard);
		}
	}
}

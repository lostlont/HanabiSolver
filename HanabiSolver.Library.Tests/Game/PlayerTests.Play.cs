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
		public void PlayOneAddsToCorrespondingEmptySuite()
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, Number.One);

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Top)
				.Returns((Card?)null);
			playerBuilder.TableBuilder.PlayedCards[suite] = pile.Object;
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			pile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Fact]
		public void PlayNextAddsToCorrespondingSuiteWithFollowingCardOnTop()
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, Number.Two);

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.One));
			playerBuilder.TableBuilder.PlayedCards[suite] = pile.Object;
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			pile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
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
			var topCardPlayed = lastNumber.HasValue
				? new Card(suite, lastNumber.Value)
				: null;

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var fuseTokens = new Mock<ITokens>();
			playerBuilder.TableBuilder.FuseTokens = fuseTokens.Object;
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Top)
				.Returns(topCardPlayed);
			playerBuilder.TableBuilder.PlayedCards[suite] = pile.Object;
			var discardPile = new Mock<IPile>();
			playerBuilder.TableBuilder.DiscardPile = discardPile.Object;
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			pile.Verify(p => p.Add(It.IsAny<Card>()), Times.Never);
			fuseTokens.Verify(t => t.Replenish(), Times.Once);
			discardPile.Verify(p => p.Add(It.Is<Card>(c => c == cardToPlay)), Times.Once);
		}

		[Fact]
		public void PlayFiveReplenishesInformationToken()
		{
			const Suite suite = Suite.Blue;

			playerBuilder.Cards = new Card(suite, Number.Five).AsEnumerable();
			var informationTokens = new Mock<ITokens>();
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var pile = new Mock<IPile>();
			pile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.Four));
			playerBuilder.TableBuilder.PlayedCards[suite] = pile.Object;
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

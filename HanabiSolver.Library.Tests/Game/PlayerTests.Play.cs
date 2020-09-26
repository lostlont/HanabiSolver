using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
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
			player.Table.PlayedCards[suite].Cards.Should().BeEquivalentTo(expectedCards);
		}

		[Theory]
		[InlineData(Number.One, Number.One)]
		[InlineData(Number.Two, null)]
		[InlineData(Number.Five, Number.Three)]
		public void PlayWrongNumberGivesFuseTokenAndDiscardsCard(Number playedNumber, Number? lastNumber)
		{
			const Suite suite = Suite.Blue;
			var cardToPlay = new Card(suite, playedNumber);
			var cardsPlayed = lastNumber
				.ExistingAsEnumerable()
				.Select(n => new Card(suite, n));

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			playerBuilder.TableBuilder.PlayedCardsBuilder[suite] = () => new Pile(cardsPlayed);
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[suite].Cards.Should().BeEquivalentTo(cardsPlayed);
			player.Table.FuseTokens.Amount.Should().Be(1);
			player.Table.DiscardPile.Top.Should().Be(cardToPlay);
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
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 0);
			playerBuilder.TableBuilder.PlayedCardsBuilder[suite] = () => new Pile(cardsPlayed);
			var player = playerBuilder.Build();

			player.Play(player.Cards.Single());

			player.Table.InformationTokens.Amount.Should().Be(1);
		}

		[Fact]
		public void PlayDrawsFromDeck()
		{
			var player = playerBuilder.Build();

			player.Play(player.Cards.First());

			var expectedDeck = new Deck(cardsInDeck);
			expectedDeck.Draw();

			player.Table.Deck.Cards.Should().Equal(expectedDeck.Cards);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(2)]
		public void PlayMovesFromTopOfDeckToBeginningOfHand(int cardIndexToPlay)
		{
			var player = playerBuilder.Build();
			var newCard = player.Table.Deck.Top;

			player.Play(cardsInHand[cardIndexToPlay]);

			var newCards = newCard.AsEnumerable();
			var oldCards = cardsInHand.ExceptAt(cardIndexToPlay);
			var expectedCards = Enumerable.Concat(newCards, oldCards);
			player.Cards.Should().Equal(expectedCards);
		}
	}
}

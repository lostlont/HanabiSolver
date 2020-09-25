using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public partial class PlayerTests
	{
		[Fact]
		public void PlayOneDropsToTopOfCorrespondingEmptySuite()
		{
			var cardToPlay = new Card(Suite.Blue, Number.One);

			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[Suite.Blue].Top.Should().Be(cardToPlay);
		}

		[Fact]
		public void PlayNextDropsToTopOfCorrespondingSuiteWithFollowingCardOnTop()
		{
			var cardsPlayed = new Card(Suite.Blue, Number.One).AsEnumerable();
			var cardToPlay = new Card(Suite.Blue, Number.Two);

			playerBuilder.TableBuilder.PlayedCardsBuilder[Suite.Blue] = () => new Pile(cardsPlayed);
			playerBuilder.Cards = cardToPlay.AsEnumerable();
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			var expectedCards = cardsPlayed.Append(cardToPlay);
			player.Table.PlayedCards[Suite.Blue].Cards.Should().BeEquivalentTo(expectedCards);
		}

		[Theory]
		[InlineData(Number.One, Number.One)]
		[InlineData(Number.Two, null)]
		public void PlayWrongNumberGivesFuseTokenAndDiscardsCard(Number playedNumber, Number? lastNumber)
		{
			var cardToPlay = new Card(Suite.Blue, playedNumber);
			var cardsPlayed = lastNumber
				.ExistingAsEnumerable()
				.Select(n => new Card(Suite.Blue, n));

			playerBuilder.TableBuilder.PlayedCardsBuilder[cardToPlay.Suite] = () => new Pile(cardsPlayed);
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[cardToPlay.Suite].Cards.Should().BeEquivalentTo(cardsPlayed);
			player.Table.FuseTokens.Amount.Should().Be(1);
			player.Table.DiscardPile.Top.Should().Be(cardToPlay);
		}
	}
}

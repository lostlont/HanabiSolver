using FluentAssertions;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
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
			playerBuilder.Cards = new List<Card>
			{
				cardToPlay,
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[Suite.Blue].Top.Should().Be(cardToPlay);
		}

		[Fact]
		public void PlayNextDropsToTopOfCorrespondingSuiteWithFollowingCardOnTop()
		{
			var cardsPlayed = new List<Card>
			{
				new Card(Suite.Blue, Number.One),
			};
			var cardToPlay = new Card(Suite.Blue, Number.Two);
			playerBuilder.TableBuilder.PlayedCardsBuilder[Suite.Blue] = () => new Pile(cardsPlayed);
			playerBuilder.Cards = new List<Card>
			{
				cardToPlay,
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			var expectedCards = cardsPlayed.Append(cardToPlay);
			player.Table.PlayedCards[Suite.Blue].Cards.Should().BeEquivalentTo(expectedCards);
		}

		[Fact]
		public void PlayAlreadyPlayedGivesFuseTokenAndDiscardsCard()
		{
			var cardToPlay = new Card(Suite.Blue, Number.One);
			var cardsPlayed = new List<Card>
			{
				cardToPlay,
			};
			playerBuilder.TableBuilder.PlayedCardsBuilder[cardToPlay.Suite] = () => new Pile(cardsPlayed);
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			player.Table.PlayedCards[cardToPlay.Suite].Cards.Should().BeEquivalentTo(cardsPlayed);
			player.Table.FuseTokens.Amount.Should().Be(1);
			player.Table.DiscardPile.Top.Should().Be(cardToPlay);
		}
	}
}

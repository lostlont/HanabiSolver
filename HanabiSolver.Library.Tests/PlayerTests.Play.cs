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
			var playedCards = new List<Card>
			{
				new Card(Suite.Blue, Number.One),
			};
			var cardToPlay = new Card(Suite.Blue, Number.Two);
			playerBuilder.TableBuilder.PlayedCardsBuilder[Suite.Blue] = () => new Pile(playedCards);
			playerBuilder.Cards = new List<Card>
			{
				cardToPlay,
			};
			var player = playerBuilder.Build();

			player.Play(cardToPlay);

			var expectedCards = playedCards.Append(cardToPlay);
			player.Table.PlayedCards[Suite.Blue].Cards.Should().BeEquivalentTo(expectedCards);
		}
	}
}

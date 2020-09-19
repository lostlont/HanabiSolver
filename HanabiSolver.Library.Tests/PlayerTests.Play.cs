using FluentAssertions;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Library.Tests
{
	public partial class PlayerTests
	{
		[Fact]
		public void PlayedCardGoesToCorrespondingSuitesTop()
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
	}
}

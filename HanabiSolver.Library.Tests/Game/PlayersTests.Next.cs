using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayersTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public void NextReturnsNextInLine(int index)
		{
			var players = new Players(playerList);

			players.Next(playerList[index]).Should().Be(playerList[index + 1]);
		}

		[Fact]
		public void NextReturnsFirstForLast()
		{
			var players = new Players(playerList);

			players.Next(playerList.Last()).Should().Be(playerList.First());
		}

		[Fact]
		public void NextThrowsForInvalidPlayer()
		{
			var players = new Players(playerList);
			var invalidPlayer = new PlayerBuilder().Build();

			players
				.Invoking(p => p.Next(invalidPlayer))
				.Should()
				.Throw<InvalidOperationException>();
		}
	}
}

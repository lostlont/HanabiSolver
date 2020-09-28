using FluentAssertions;
using HanabiSolver.Library.Game;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void CanGiveInformationForExistingSuite()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingSuite()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.Red).Should().BeFalse();
		}

		// TODO CanGiveInformationForPartiallyInformedSuite
		// TODO CanNotGiveInformationForFullyInformedSuite

		[Fact]
		public void CanGiveInformationForExistingNumber()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingNumber()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.Five).Should().BeFalse();
		}

		// TODO CanGiveInformationForPartiallyInformedNumber
		// TODO CanNotGiveInformationForFullyInformedNumber
	}
}

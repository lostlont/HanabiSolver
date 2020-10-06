using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		// TODO Instead of otherPlayer a mock should be used!
		[Fact]
		public void CanGiveInformationForExistingSuite()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingSuite()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.Red).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForPartiallyInformedSuite()
		{
			var player = new PlayerBuilder().Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.White, Number.Two),
			};
			var otherPlayerInformation = new Dictionary<Card, Information>
			{
				[otherPlayerCards[0]] = new Information { IsSuiteKnown = true },
				[otherPlayerCards[1]] = new Information { IsSuiteKnown = false },
			};
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
				InformationBuilder = card => otherPlayerInformation[card],
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedSuite()
		{
			var player = new PlayerBuilder().Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.White, Number.Two),
			};
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
				InformationBuilder = card => new Information { IsSuiteKnown = true },
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForSuiteWithNoInformationTokens()
		{
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = new Mock<ITokens>().Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new PlayerBuilder().Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForExistingNumber()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingNumber()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.Five).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForPartiallyInformedNumber()
		{
			var player = new PlayerBuilder().Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.Yellow, Number.One),
			};
			var otherPlayerInformation = new Dictionary<Card, Information>
			{
				[otherPlayerCards[0]] = new Information { IsSuiteKnown = true },
				[otherPlayerCards[1]] = new Information { IsSuiteKnown = false },
			};
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
				InformationBuilder = card => otherPlayerInformation[card],
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedNumber()
		{
			var player = new PlayerBuilder().Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.Yellow, Number.One),
			};
			var otherPlayerBuilder = new PlayerBuilder
			{
				Cards = otherPlayerCards,
				InformationBuilder = card => new Information { IsNumberKnown = true },
			};
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForNumberWithNoInformationTokens()
		{
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = new Mock<ITokens>().Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new PlayerBuilder().Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeFalse();
		}
	}
}

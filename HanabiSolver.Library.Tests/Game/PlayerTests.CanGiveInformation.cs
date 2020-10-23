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
		[Fact]
		public void CanGiveInformationForExistingSuite()
		{
			const Suite ownedSuite = Suite.White;
			var ownedCard = new Card(ownedSuite, Number.One);

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());

			player.CanGiveInformation(otherPlayer.Object, ownedSuite).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingSuite()
		{
			const Suite nonOwnedSuite = Suite.Red;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>());

			player.CanGiveInformation(otherPlayer.Object, nonOwnedSuite).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForPartiallyInformedSuite()
		{
			const Suite suite = Suite.White;
			var informedCard = new Card(suite, Number.One);
			var nonInformedCard = new Card(suite, Number.Two);
			
			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>
				{
					informedCard,
					nonInformedCard,
				});
			otherPlayer
				.Setup(p => p.Information)
				.Returns(new Dictionary<Card, Information>
				{
					[informedCard] = new Information { IsSuiteKnown = true },
					[nonInformedCard] = new Information { IsSuiteKnown = false },
				});

			player.CanGiveInformation(otherPlayer.Object, suite).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedSuite()
		{
			const Suite suite = Suite.White;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>
				{
					new Card(suite, Number.One),
					new Card(suite, Number.Two),
				});
			otherPlayer
				.Setup(p => p.Information[It.IsAny<Card>()])
				.Returns(new Information { IsSuiteKnown = true });

			player.CanGiveInformation(otherPlayer.Object, suite).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForSuiteWithNoInformationTokens()
		{
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(i => i.Amount)
				.Returns(0);
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);

			player.CanGiveInformation(otherPlayer.Object, Suite.White).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForExistingNumber()
		{
			const Number ownedNumber = Number.One;
			var ownedCard = new Card(Suite.White, ownedNumber);

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());

			player.CanGiveInformation(otherPlayer.Object, ownedNumber).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNonExistingNumber()
		{
			const Number nonOwnedNumber = Number.Five;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>());

			player.CanGiveInformation(otherPlayer.Object, nonOwnedNumber).Should().BeFalse();
		}

		[Fact]
		public void CanGiveInformationForPartiallyInformedNumber()
		{
			const Number number = Number.One;
			var informedCard = new Card(Suite.White, number);
			var nonInformedCard = new Card(Suite.Yellow, number);
			
			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>
				{
					informedCard,
					nonInformedCard,
				});
			otherPlayer
				.Setup(p => p.Information)
				.Returns(new Dictionary<Card, Information>
				{
					[informedCard] = new Information { IsNumberKnown = true },
					[nonInformedCard] = new Information { IsNumberKnown = false },
				});

			player.CanGiveInformation(otherPlayer.Object, number).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedNumber()
		{
			const Number number = Number.One;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>
				{
					new Card(Suite.White, number),
					new Card(Suite.Yellow, number),
				});
			otherPlayer
				.Setup(p => p.Information[It.IsAny<Card>()])
				.Returns(new Information { IsNumberKnown = true });

			player.CanGiveInformation(otherPlayer.Object, number).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForNumberWithNoInformationTokens()
		{
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(i => i.Amount)
				.Returns(0);
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);

			player.CanGiveInformation(otherPlayer.Object, Number.One).Should().BeFalse();
		}
	}
}

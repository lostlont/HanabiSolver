using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void GiveInformationWithSuiteUsesInformationToken()
		{
			const Suite suite = Suite.White;
			var ownedCard = new Card(suite, Number.One);
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			informationTokens
				.Setup(t => t.Use());
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());

			player.GiveInformation(otherPlayer.Object, suite);

			informationTokens.Verify(t => t.Use(), Times.Once);
		}

		[Fact]
		public void GiveInformationWithSuiteThrowsForNoInformationToken()
		{
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
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

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Suite.White))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithSuiteThrowsForNoSuchSuite()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>());

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Suite.Red))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithSuiteSetsSuiteKnownOnCardsInSameSuite()
		{
			var ownedCard = new Card(Suite.White, Number.One);
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			informationTokens
				.Setup(t => t.Use());
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			var information = new Information();
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(information);

			player.GiveInformation(otherPlayer.Object, ownedCard.Suite);

			information.IsSuiteKnown.Should().BeTrue();
		}

		[Fact]
		public void GiveInformationWithNumberUsesInformationToken()
		{
			const Number number = Number.One;
			var ownedCard = new Card(Suite.White, number);
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			informationTokens
				.Setup(t => t.Use());
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());

			player.GiveInformation(otherPlayer.Object, number);

			informationTokens.Verify(t => t.Use(), Times.Once);
		}

		[Fact]
		public void GiveInformationWithNumberThrowsForNoInformationToken()
		{
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
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

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Number.One))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithNumberThrowsForNoSuchNumber()
		{
			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card>());

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Number.Five))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithNumberSetsNumberKnownOnCardsWithSameNumber()
		{
			var ownedCard = new Card(Suite.White, Number.One);
			var informationTokens = new Mock<ITokens>(MockBehavior.Strict);
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			informationTokens
				.Setup(t => t.Use());
			var playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokens = informationTokens.Object,
				},
			};
			var player = playerBuilder.Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			var information = new Information();
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(information);

			player.GiveInformation(otherPlayer.Object, ownedCard.Number);

			information.IsNumberKnown.Should().BeTrue();
		}
	}
}

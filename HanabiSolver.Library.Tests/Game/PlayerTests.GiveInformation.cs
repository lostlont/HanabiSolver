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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());
			otherPlayer
				.Setup(p => p.ReceiveInformation(suite));

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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Suite.White))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithSuiteCallsReceiveInformation()
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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());
			otherPlayer
				.Setup(p => p.ReceiveInformation(ownedCard.Suite));

			player.GiveInformation(otherPlayer.Object, ownedCard.Suite);

			otherPlayer.Verify(p => p.ReceiveInformation(ownedCard.Suite), Times.Once);
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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());
			otherPlayer
				.Setup(p => p.ReceiveInformation(number));

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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);

			player
				.Invoking(p => p.GiveInformation(otherPlayer.Object, Number.One))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithNumberCallsReceiveInformation()
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
			var otherPlayer = new Mock<IInformationReceiverReadOnlyPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.Cards)
				.Returns(new List<Card> { ownedCard });
			otherPlayer
				.Setup(p => p.Information[ownedCard])
				.Returns(new Information());
			otherPlayer
				.Setup(p => p.ReceiveInformation(ownedCard.Number));

			player.GiveInformation(otherPlayer.Object, ownedCard.Number);

			otherPlayer.Verify(p => p.ReceiveInformation(ownedCard.Number), Times.Once);
		}
	}
}

using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		[Fact]
		public void InformationAffectedCardsReturnsCardsInSameSuite()
		{
			const Suite ownedSuite = Suite.White;
			var ownedCard = new Card(ownedSuite, Number.One);
			var ownedCards = new List<Card> { ownedCard };

			var player = new PlayerBuilder
			{
				Cards = ownedCards
			}.Build();

			player.InformationAffectedCards(ownedSuite).Should().Equal(ownedCards);
		}

		[Fact]
		public void InformationAffectedCardsReturnsEmptyForNonExistingSuite()
		{
			const Suite nonOwnedSuite = Suite.Red;

			var player = new PlayerBuilder
			{
				Cards = new List<Card> { new Card(Suite.White, Number.One) },
			}.Build();

			player.InformationAffectedCards(nonOwnedSuite).Should().BeEmpty();
		}

		[Fact]
		public void InformationAffectedCardsReturnsCardsWithNoInformation()
		{
			const Suite suite = Suite.White;
			var informedCard = new Card(suite, Number.One);
			var nonInformedCard = new Card(suite, Number.Two);
			
			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					informedCard,
					nonInformedCard,
				},
			}.Build();
			player.Information[informedCard].IsSuiteKnown = true;

			var expectedCards = nonInformedCard.AsEnumerable();
			player.InformationAffectedCards(suite).Should().Equal(expectedCards);
		}

		[Fact]
		public void InformationAffectedCardsReturnsEmptyForFullyInformedSuite()
		{
			const Suite suite = Suite.White;

			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					new Card(suite, Number.One),
					new Card(suite, Number.Two),
				},
			}.Build();

			foreach (var card in player.Cards)
				player.Information[card].IsSuiteKnown = true;

			player.InformationAffectedCards(suite).Should().BeEmpty();
		}

		[Fact]
		public void CanNotGiveInformationForSuiteWithNoInformationTokens()
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
				.As<IReadOnlyPlayer>()
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
				.As<IReadOnlyPlayer>()
				.Setup(p => p.Information)
				.Returns(new Dictionary<Card, IReadOnlyInformation>
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
				.As<IReadOnlyPlayer>()
				.Setup(p => p.Information[It.IsAny<Card>()])
				.Returns(new Information { IsNumberKnown = true });

			player.CanGiveInformation(otherPlayer.Object, number).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForNumberWithNoInformationTokens()
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

			player.CanGiveInformation(otherPlayer.Object, Number.One).Should().BeFalse();
		}
	}
}

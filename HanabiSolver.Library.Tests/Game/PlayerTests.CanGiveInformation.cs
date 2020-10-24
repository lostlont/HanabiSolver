using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
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
		public void CanGiveInformationForSuiteIfHasAffectedCards()
		{
			const Suite suite = Suite.White;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.InformationAffectedCards(suite))
				.Returns(new Card(suite, Number.One).AsEnumerable());

			player.CanGiveInformation(otherPlayer.Object, suite).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForSuiteIfDoesNotHaveAffectedCards()
		{
			const Suite suite = Suite.White;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.InformationAffectedCards(suite))
				.Returns(Enumerable.Empty<Card>());

			player.CanGiveInformation(otherPlayer.Object, suite).Should().BeFalse();
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

		[Fact]
		public void CanGiveInformationForNumberIfHasAffectedCards()
		{
			const Number number = Number.One;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.InformationAffectedCards(number))
				.Returns(new Card(Suite.White, number).AsEnumerable());

			player.CanGiveInformation(otherPlayer.Object, number).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForNumberIfDoesNotHaveAffectedCards()
		{
			const Number number = Number.One;

			var player = new PlayerBuilder().Build();
			var otherPlayer = new Mock<IPlayer>(MockBehavior.Strict);
			otherPlayer
				.Setup(p => p.InformationAffectedCards(number))
				.Returns(Enumerable.Empty<Card>());

			player.CanGiveInformation(otherPlayer.Object, number).Should().BeFalse();
		}
	}
}

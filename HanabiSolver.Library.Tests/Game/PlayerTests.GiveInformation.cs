using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		private readonly PlayerBuilder otherPlayerBuilder = new PlayerBuilder
		{
			Cards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.Yellow, Number.Two),
				new Card(Suite.Green, Number.Three),
				new Card(Suite.Yellow, Number.One),
			},
		};

		[Fact]
		public void GiveInformationWithSuiteUsesInformationToken()
		{
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.GiveInformation(otherPlayer, Suite.White);

			informationTokens.Verify(t => t.Use(), Times.Once);
		}

		[Fact]
		public void GiveInformationWithSuiteThrowsForNoInformationToken()
		{
			// TODO Strict mode?
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(0);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player
				.Invoking(p => p.GiveInformation(otherPlayer, Suite.White))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithSuiteThrowsForNoSuchSuite()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player
				.Invoking(p => p.GiveInformation(otherPlayer, Suite.Red))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithSuiteSetsSuiteKnownOnCardsInSameSuite()
		{
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			var card = otherPlayer.Cards.First();

			player.GiveInformation(otherPlayer, card.Suite);

			otherPlayer.Information[card].IsSuiteKnown.Should().BeTrue();
		}

		[Fact]
		public void GiveInformationWithNumberUsesInformationToken()
		{
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.GiveInformation(otherPlayer, Number.One);

			informationTokens.Verify(t => t.Use(), Times.Once);
		}

		[Fact]
		public void GiveInformationWithNumberThrowsForNoInformationToken()
		{
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(0);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player
				.Invoking(p => p.GiveInformation(otherPlayer, Number.One))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithNumberThrowsForNoSuchNumber()
		{
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player
				.Invoking(p => p.GiveInformation(otherPlayer, Number.Five))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void GiveInformationWithNumberSetsNumberKnownOnCardsWithSameNumber()
		{
			var informationTokens = new Mock<ITokens>();
			informationTokens
				.Setup(t => t.Amount)
				.Returns(1);
			playerBuilder.TableBuilder.InformationTokens = informationTokens.Object;
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			var card = otherPlayer.Cards.First();

			player.GiveInformation(otherPlayer, card.Number);

			otherPlayer.Information[card].IsNumberKnown.Should().BeTrue();
		}
	}
}

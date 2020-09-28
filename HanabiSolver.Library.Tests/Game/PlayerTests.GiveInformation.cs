using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
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
			CardsBuilder = () => new List<Card>
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
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 1);
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.GiveInformation(otherPlayer, Suite.White);

			player.Table.InformationTokens.Amount.Should().Be(0);
		}

		[Fact]
		public void GiveInformationWithSuiteThrowsForNoInformationToken()
		{
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 0);
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
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			var card = otherPlayer.Cards.First();

			player.GiveInformation(otherPlayer, card.Suite);

			otherPlayer.Information[card].IsSuiteKnown.Should().BeTrue();
		}

		[Fact]
		public void GiveInformationWithNumberUsesInformationToken()
		{
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 1);
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.GiveInformation(otherPlayer, Number.One);

			player.Table.InformationTokens.Amount.Should().Be(0);
		}

		[Fact]
		public void GiveInformationWithNumberThrowsForNoInformationToken()
		{
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 0);
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
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			var card = otherPlayer.Cards.First();

			player.GiveInformation(otherPlayer, card.Number);

			otherPlayer.Information[card].IsNumberKnown.Should().BeTrue();
		}

		// TODO Test Discard and play should get rid of information
	}
}

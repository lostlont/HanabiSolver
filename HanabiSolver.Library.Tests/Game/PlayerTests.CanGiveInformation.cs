using FluentAssertions;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
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

		[Fact]
		public void CanGiveInformationForPartiallyInformedSuite()
		{
			var player = playerBuilder.Build();

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
			otherPlayerBuilder.CardsBuilder = () => otherPlayerCards;
			otherPlayerBuilder.InformationBuilder = card => otherPlayerInformation[card];
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedSuite()
		{
			var player = playerBuilder.Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.White, Number.Two),
			};
			otherPlayerBuilder.CardsBuilder = () => otherPlayerCards;
			otherPlayerBuilder.InformationBuilder = card => new Information { IsSuiteKnown = true };
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForSuiteWithNoInformationTokens()
		{
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 0);
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Suite.White).Should().BeFalse();
		}

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

		[Fact]
		public void CanGiveInformationForPartiallyInformedNumber()
		{
			var player = playerBuilder.Build();

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
			otherPlayerBuilder.CardsBuilder = () => otherPlayerCards;
			otherPlayerBuilder.InformationBuilder = card => otherPlayerInformation[card];
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeTrue();
		}

		[Fact]
		public void CanNotGiveInformationForFullyInformedNumber()
		{
			var player = playerBuilder.Build();

			var otherPlayerCards = new List<Card>
			{
				new Card(Suite.White, Number.One),
				new Card(Suite.Yellow, Number.One),
			};
			otherPlayerBuilder.CardsBuilder = () => otherPlayerCards;
			otherPlayerBuilder.InformationBuilder = card => new Information { IsNumberKnown = true };
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeFalse();
		}

		[Fact]
		public void CanNotGiveInformationForNumberWithNoInformationTokens()
		{
			playerBuilder.TableBuilder.InformationTokensBuilder = () => new Tokens(3, 0);
			var player = playerBuilder.Build();
			var otherPlayer = otherPlayerBuilder.Build();

			player.CanGiveInformation(otherPlayer, Number.One).Should().BeFalse();
		}
	}
}

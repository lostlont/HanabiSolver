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
		[Fact]
		public void ReceiveInformationWithSuiteSetsSuiteKnownOnCardsInSameSuite()
		{
			var cardInSameSuite = new Card(Suite.White, Number.One);
			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					cardInSameSuite,
					new Card(Suite.Yellow, Number.One),
				},
			}.Build();

			player.ReceiveInformation(Suite.White);

			player.Information[cardInSameSuite].IsSuiteKnown.Should().BeTrue();
		}

		[Fact]
		public void ReceiveInformationWithSuiteDoesNotSetSuiteKnownOnCardsInDifferentSuite()
		{
			var cardInDifferentSuite = new Card(Suite.Yellow, Number.One);
			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					new Card(Suite.White, Number.One),
					cardInDifferentSuite,
				},
			}.Build();

			player.ReceiveInformation(Suite.White);

			player.Information[cardInDifferentSuite].IsSuiteKnown.Should().BeFalse();
		}

		[Fact]
		public void ReceiveInformationWithSuiteThrowsForNoCardsAffected()
		{
			var player = new PlayerBuilder
			{
				Cards = Enumerable.Empty<Card>(),
			}.Build();

			player
				.Invoking(p => p.ReceiveInformation(Suite.White))
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Fact]
		public void ReceiveInformationWithNumberSetsNumberKnownOnCardsWithSameNumber()
		{
			var cardWithSameNumber = new Card(Suite.White, Number.One);
			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					cardWithSameNumber,
					new Card(Suite.White, Number.Two),
				},
			}.Build();

			player.ReceiveInformation(Number.One);

			player.Information[cardWithSameNumber].IsNumberKnown.Should().BeTrue();
		}

		[Fact]
		public void ReceiveInformationWithNumberDoesNotSetNumberKnownOnCardsWithDifferentNumber()
		{
			var cardWithDifferentNumber = new Card(Suite.White, Number.Two);
			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					new Card(Suite.White, Number.One),
					cardWithDifferentNumber,
				},
			}.Build();

			player.ReceiveInformation(Number.One);

			player.Information[cardWithDifferentNumber].IsNumberKnown.Should().BeFalse();
		}

		[Fact]
		public void ReceiveInformationWithNumberThrowsForNoCardsAffected()
		{
			var player = new PlayerBuilder
			{
				Cards = Enumerable.Empty<Card>(),
			}.Build();

			player
				.Invoking(p => p.ReceiveInformation(Number.One))
				.Should()
				.Throw<InvalidOperationException>();
		}
	}
}

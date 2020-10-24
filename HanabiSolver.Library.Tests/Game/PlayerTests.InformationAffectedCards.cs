using FluentAssertions;
using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
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
			var player = new PlayerBuilder
			{
				Cards = new List<Card> { new Card(Suite.White, Number.One) },
			}.Build();

			player.InformationAffectedCards(Suite.Red).Should().BeEmpty();
		}

		[Fact]
		public void InformationAffectedCardsReturnsCardsWithNoSuiteInformation()
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
		public void InformationAffectedCardsReturnsCardsWithSameNumber()
		{
			const Number ownedNumber = Number.One;
			var ownedCard = new Card(Suite.White, ownedNumber);
			var ownedCards = new List<Card> { ownedCard };

			var player = new PlayerBuilder
			{
				Cards = ownedCards
			}.Build();

			player.InformationAffectedCards(ownedNumber).Should().Equal(ownedCards);
		}

		[Fact]
		public void InformationAffectedCardsReturnsEmptyForNonExistingNumber()
		{
			var player = new PlayerBuilder
			{
				Cards = new List<Card> { new Card(Suite.White, Number.One) },
			}.Build();

			player.InformationAffectedCards(Number.Five).Should().BeEmpty();
		}

		[Fact]
		public void InformationAffectedCardsReturnsCardsWithNoNumberInformation()
		{
			const Number number = Number.One;
			var informedCard = new Card(Suite.White, number);
			var nonInformedCard = new Card(Suite.Yellow, number);

			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					informedCard,
					nonInformedCard,
				},
			}.Build();
			player.Information[informedCard].IsNumberKnown = true;

			var expectedCards = nonInformedCard.AsEnumerable();
			player.InformationAffectedCards(number).Should().Equal(expectedCards);
		}

		[Fact]
		public void InformationAffectedCardsReturnsEmptyForFullyInformedNumber()
		{
			const Number number = Number.One;

			var player = new PlayerBuilder
			{
				Cards = new List<Card>
				{
					new Card(Suite.White, number),
					new Card(Suite.Yellow, number),
				},
			}.Build();

			foreach (var card in player.Cards)
				player.Information[card].IsNumberKnown = true;

			player.InformationAffectedCards(number).Should().BeEmpty();
		}
	}
}

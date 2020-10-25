using HanabiSolver.Library.Game;
using Moq;
using System.Linq;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class GameManagerTests
	{
		private IPile BuildPile(Suite suite, Number topNumber)
		{
			var result = new Mock<IPile>(MockBehavior.Strict);
			result
				.Setup(p => p.Top)
				.Returns(new Card(suite, topNumber));
			return result.Object;
		}

		private IPile BuildFinishingPile(Suite suite, int turnsUntilFinish)
		{
			var playedCards = new Mock<IPile>(MockBehavior.Strict);
			var topSequence = playedCards.SetupSequence(p => p.Top);

			var card = new Card(suite, Number.Four);
			foreach (var turnIndex in Enumerable.Range(0, turnsUntilFinish))
				topSequence.Returns(card);

			var lastCard = new Card(suite, Number.Five);
			topSequence
				.Returns(lastCard)
				.Returns(lastCard);

			return playedCards.Object;
		}

		private IPile BuildFullPile(Suite suite)
		{
			var pile = new Mock<IPile>(MockBehavior.Strict);
			pile
				.Setup(p => p.Top)
				.Returns(new Card(suite, Number.Five));
			return pile.Object;
		}

		private IPile BuildEmptyPile()
		{
			var pile = new Mock<IPile>(MockBehavior.Strict);
			pile
				.Setup(p => p.Top)
				.Returns<Card?>(null);
			return pile.Object;
		}
	}
}

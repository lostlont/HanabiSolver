using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		private readonly IReadOnlyList<Card> cardsInHand;
		private readonly IReadOnlyList<Card> cardsInDeck;
		private readonly PlayerBuilder playerBuilder;

		public PlayerTests()
		{
			cardsInHand = new List<Card>
			{
				new Card(Suite.White, Number.Five),
				new Card(Suite.White, Number.One),
				new Card(Suite.Red, Number.Three),
			};

			cardsInDeck = new List<Card>
			{
				new Card(Suite.Red, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Blue, Number.Three),
			};

			playerBuilder = new PlayerBuilder
			{
				CardsBuilder = () => cardsInHand,
				TableBuilder = new TableBuilder
				{
					DeckBuilder = () => new Deck(cardsInDeck),
					InformationTokensBuilder = () => new Tokens(3, 1),
				},
			};
		}

		// TODO Test giving information.
	}
}

using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using System.Collections.Generic;

namespace HanabiSolver.Library.Tests.Game
{
	public partial class PlayerTests
	{
		// TODO Remove these.
		private readonly IReadOnlyList<Card> cardsInDeck;
		private readonly PlayerBuilder playerBuilder;

		public PlayerTests()
		{
			cardsInDeck = new List<Card>
			{
				new Card(Suite.Red, Number.One),
				new Card(Suite.Green, Number.Two),
				new Card(Suite.Blue, Number.Three),
			};

			playerBuilder = new PlayerBuilder
			{
				TableBuilder = new TableBuilder
				{
					InformationTokensBuilder = () => new Tokens(3, 1),
				},
			};
		}
	}
}

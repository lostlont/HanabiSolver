using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayStandaloneClued : ITactics
	{
		private readonly InformationMemento informationMemento = new InformationMemento();

		public PlayStandaloneClued(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			var hasNewInformation = informationMemento.Update(gameState.CurrentPlayer);
			var hasOneNewInformation = false;
			var hasUnknownCardsToTheRight = false;

			if (hasNewInformation)
			{
				hasOneNewInformation = informationMemento.GetNewInformation(gameState.CurrentPlayer).Count == 1;

				if (hasOneNewInformation)
				{
					var leftmostCluedCard = LeftmostCluedCard(gameState.CurrentPlayer);

					hasUnknownCardsToTheRight = gameState.CurrentPlayer.Cards
						.SkipWhile(c => c != leftmostCluedCard)
						.Skip(1)
						.Any(c => Utils.IsUnknownCard(gameState.CurrentPlayer, c));
				}
			}

			return hasOneNewInformation && hasUnknownCardsToTheRight;
		}

		public void Apply(IGameState gameState)
		{
			var leftmostCluedCard = LeftmostCluedCard(gameState.CurrentPlayer);

			var cardNumber = gameState.CurrentPlayer.CardNumber(leftmostCluedCard);
			var playerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			Log.Write($"Player {playerNumber} applies {nameof(PlayStandaloneClued)} because card ");
			Log.Info(leftmostCluedCard);
			Log.Write($", card {cardNumber} was clued and cards without clue are present on its right.");

			gameState.CurrentPlayer.Play(leftmostCluedCard);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
		}

		private Card LeftmostCluedCard(IPlayer player)
		{
			return informationMemento
				.GetNewInformation(player)
				.First();
		}
	}
}

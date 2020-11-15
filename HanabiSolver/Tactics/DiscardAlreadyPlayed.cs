using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class DiscardAlreadyPlayed : ITactics
	{
		public DiscardAlreadyPlayed(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any(c => IsAlreadyPlayed(gameState, c));
		}

		public void Apply(IGameState gameState)
		{
			var playableCard = gameState.CurrentPlayer.Cards.First(c => IsAlreadyPlayed(gameState, c));

			var cardNumber = gameState.CurrentPlayer.CardNumber(playableCard);
			var playerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			Log.Write($"Player {playerNumber} applies {nameof(DiscardAlreadyPlayed)} because card ");
			Log.Info(playableCard);
			Log.WriteLine($", card {cardNumber} is already played.");

			gameState.CurrentPlayer.Discard(playableCard);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
		}

		private bool IsAlreadyPlayed(IGameState gameState, Card card)
		{
			var isKnown = Utils.IsKnownCard(gameState.CurrentPlayer, card);
			var isPlayed = gameState.Table.PlayedCards[card.Suite].Cards.Select(c => c.Number).Contains(card.Number);

			return isKnown && isPlayed;
		}
	}
}

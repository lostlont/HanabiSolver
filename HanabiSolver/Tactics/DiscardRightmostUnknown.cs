using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class DiscardRightmostUnknown : ITactics
	{
		public DiscardRightmostUnknown(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
			=> HasUnknownCards(gameState.CurrentPlayer);

		public void Apply(IGameState gameState)
		{
			var rightmostUnknownCard = RightmostUnknownCard(gameState.CurrentPlayer)!;

			var cardNumber = gameState.CurrentPlayer.CardNumber(rightmostUnknownCard);
			var playerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			Log.Write($"Player {playerNumber} applies {nameof(DiscardRightmostUnknown)} because card ");
			Log.Info(rightmostUnknownCard);
			Log.WriteLine($", card {cardNumber} does not have any clues.");

			gameState.CurrentPlayer.Discard(rightmostUnknownCard);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
		}

		private bool HasUnknownCards(IReadOnlyPlayer player)
			=> RightmostUnknownCard(player) != null;

		private Card? RightmostUnknownCard(IReadOnlyPlayer player)
		{
			return player.Cards
				.Reverse()
				.FirstOrDefault(c => Utils.IsUnknownCard(player, c));
		}
	}
}

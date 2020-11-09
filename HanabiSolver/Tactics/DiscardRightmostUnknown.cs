using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class DiscardRightmostUnknown : ITactics
	{
		public bool CanApply(IGameState gameState)
			=> HasUnknownCards(gameState.CurrentPlayer);

		public void Apply(IGameState gameState)
		{
			var rightmostUnknownCard = RightmostUnknownCard(gameState.CurrentPlayer)!;

			gameState.CurrentPlayer.Discard(rightmostUnknownCard);
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

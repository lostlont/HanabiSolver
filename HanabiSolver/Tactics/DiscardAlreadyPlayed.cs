using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class DiscardAlreadyPlayed : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any(c => IsAlreadyPlayed(gameState, c));
		}

		public void Apply(IGameState gameState)
		{
			var playableCard = gameState.CurrentPlayer.Cards.First(c => IsAlreadyPlayed(gameState, c));

			gameState.CurrentPlayer.Discard(playableCard);
		}

		private bool IsAlreadyPlayed(IGameState gameState, Card card)
		{
			var isKnown = Utils.IsKnownCard(gameState.CurrentPlayer, card);
			var isPlayed = gameState.Table.PlayedCards[card.Suite].Cards.Select(c => c.Number).Contains(card.Number);

			return isKnown && isPlayed;
		}
	}
}

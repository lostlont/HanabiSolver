using HanabiSolver.Common.Extensions;
using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayPlayable : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any(c => IsPlayable(gameState, c));
		}

		public void Apply(IGameState gameState)
		{
			var playableCard = gameState.CurrentPlayer.Cards.First(c => IsPlayable(gameState, c));

			gameState.CurrentPlayer.Play(playableCard);
		}

		private bool IsPlayable(IGameState gameState, Card card)
		{
			var isKnown = Utils.IsKnownCard(gameState.CurrentPlayer, card);
			var isNext = card.Number == gameState.Table.PlayedCards[card.Suite].Top?.Number.Next();

			return isKnown && isNext;
		}
	}
}

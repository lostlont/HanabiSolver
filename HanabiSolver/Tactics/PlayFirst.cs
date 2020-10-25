using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayFirst : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any();
		}

		public void Apply(IGameState gameState)
		{
			gameState.CurrentPlayer.Play(gameState.CurrentPlayer.Cards.First());
		}
	}

}

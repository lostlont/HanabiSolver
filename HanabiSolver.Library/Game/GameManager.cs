using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class GameManager
	{
		public GameManager(GameState gameState)
		{
			GameState = gameState;
		}

		public GameState GameState { get; }
		public bool IsEnded
		{
			get
			{
				return
					(GameState.Table.FuseTokens.Amount >= GameState.Table.FuseTokens.MaxAmount) ||
					GameState.Table.PlayedCards.Values.All(pile => pile.Top?.Number == Number.Five);
			}
		}

		public void Play(IEnumerable<ITactics> tactics)
		{
			while (!IsEnded)
			{
				tactics.First().Apply(GameState.CurrentPlayer);
				GameState.CurrentPlayer = GameState.NextPlayer;
			}
		}
	}
}

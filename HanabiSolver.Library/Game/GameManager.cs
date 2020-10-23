using System;

namespace HanabiSolver.Library.Game
{
	public class GameManager
	{
		public GameManager(GameState gameState)
		{
			GameState = gameState;
		}

		public GameState GameState { get; }

		public void Simulate()
		{
		}
	}
}

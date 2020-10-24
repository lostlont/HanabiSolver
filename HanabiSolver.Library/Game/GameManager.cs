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

		public void Play()
		{
			var nextPlayer = GameState.Players
				.SkipWhile(p => p != GameState.CurrentPlayer)
				.Skip(1)
				.FirstOrDefault();
			GameState.CurrentPlayer = nextPlayer ?? GameState.Players.First();
		}
	}
}

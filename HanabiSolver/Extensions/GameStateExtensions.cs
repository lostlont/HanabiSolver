using HanabiSolver.Library.Game;

namespace HanabiSolver.Extensions
{
	public static class GameStateExtensions
	{
		public static int PlayerNumber(this IGameState gameState, IReadOnlyPlayer player)
		{
			return gameState.Players.IndexOf(player) + 1;
		}
	}
}

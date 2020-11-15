using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayFirst : ITactics
	{
		public PlayFirst(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any();
		}

		public void Apply(IGameState gameState)
		{
			var playerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			Log.Write($"Player {playerNumber} applies {nameof(PlayFirst)} because nothing else can be applied.");

			gameState.CurrentPlayer.Play(gameState.CurrentPlayer.Cards.First());

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
		}
	}
}

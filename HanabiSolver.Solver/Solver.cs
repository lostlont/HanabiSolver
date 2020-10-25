using HanabiSolver.Library.Game;
using HanabiSolver.Solver.Factories;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Solver
{
	public interface ISolver
	{
		Result Solve();
	}

	internal class Solver : ISolver
	{
		private readonly IReadOnlyList<ITactics> tactics;
		private readonly int iterationCount;
		private readonly bool isLoggingEnabled;

		internal Solver(IEnumerable<ITactics> tactics, int iterationCount, bool isLoggingEnabled)
		{
			this.tactics = tactics.ToList();
			this.iterationCount = iterationCount;
			this.isLoggingEnabled = isLoggingEnabled;
		}

		public Result Solve()
		{
			var scores = PlayAll().ToList();
			var result = Result.FromValues(scores);
			return result;
		}

		private IEnumerable<int> PlayAll()
		{
			foreach (var _ in Enumerable.Range(0, iterationCount))
				yield return Play();
		}

		private int Play()
		{
			var gameManager = GameManagerFactory.Create();
			if (isLoggingEnabled)
				Log.Info(gameManager);

			gameManager.Play(tactics);

			if (isLoggingEnabled)
				Log.Info(gameManager);

			return gameManager.Score;
		}
	}
}

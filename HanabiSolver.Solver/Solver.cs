using HanabiSolver.Library.Game;
using HanabiSolver.Solver.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HanabiSolver.Solver
{
	public interface ISolver
	{
		Result Solve();
	}

	internal class Solver : ISolver
	{
		private readonly IReadOnlyList<Func<ITactics>> tactics;
		private readonly int iterationCount;
		private readonly ILog log;

		internal Solver(IEnumerable<Func<ITactics>> tactics, int iterationCount, ILog log)
		{
			this.tactics = tactics.ToList();
			this.iterationCount = iterationCount;
			this.log = log;
		}

		public Result Solve()
		{
			var scores = PlayAll();
			var result = Result.FromValues(scores);
			return result;
		}

		private IEnumerable<int> PlayAll()
		{
			ThreadPool.GetMinThreads(out var minThreads, out var _);
			var remainingIterations = iterationCount;
			var remainingThreads = minThreads;
			var tasks = new List<Task<List<int>>>();
			foreach (var i in Enumerable.Range(0, minThreads))
			{
				var iterations = (int)Math.Round((float)remainingIterations / remainingThreads);
				if (iterations > 0)
				{
					var task = Task.Run(() =>
						Enumerable.Range(0, iterations)
							.Select(_ => Play())
							.ToList());
					tasks.Add(task);
				}

				remainingIterations -= iterations;
				remainingThreads -= 1;
			}

			return tasks
				.SelectMany(t => t.Result);
		}

		private int Play()
		{
			var gameManager = GameManagerFactory.Create();
			log.Info(gameManager);

			var tactics = this.tactics
				.Select(t => t());

			gameManager.Play(tactics);

			log.Info(gameManager);

			return gameManager.Score;
		}
	}
}

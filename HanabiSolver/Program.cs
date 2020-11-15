using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using HanabiSolver.Solver.Builders;
using HanabiSolver.Tactics;
using System;

namespace HanabiSolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int iterationCount = 100_000;

			var log = new Log { Enabled = false };

			var result = SolverBuilder.New
				.WithIterationCount(iterationCount)
				.WithTactics(new Func<ITactics>[]
				{
					() => new SaveNextChop(log),
					() => new CluePlayable(log),
					() => new PlayStandaloneClued(log),
					() => new DiscardAlreadyPlayed(log),
					() => new DiscardRightmostUnknown(log),
					() => new PlayPlayable(log),
					() => new PlayFirst(log),
				})
				.WithLogger(log)
				.Build()
				.Solve();

			Console.WriteLine($"Playing {iterationCount}:");
			Console.WriteLine($"  min     = {result.Min}");
			Console.WriteLine($"  median  = {result.Median}");
			Console.WriteLine($"  average = {result.Average}");
			Console.WriteLine($"  max     = {result.Max}");
		}
	}
}

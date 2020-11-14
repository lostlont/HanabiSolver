using HanabiSolver.Library.Game;
using HanabiSolver.Solver.Builders;
using HanabiSolver.Tactics;
using System;

namespace HanabiSolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int iterationCount = 10_000;

			var result = SolverBuilder.New
				.WithIterationCount(iterationCount)
				.WithTactics(new ITactics[]
				{
					new SaveNextChop(),
					new CluePlayable(),
					new PlayLeftmostClued(),
					new DiscardAlreadyPlayed(),
					new DiscardRightmostUnknown(),
					new PlayPlayable(),
					new PlayFirst(),
				})
				//.WithLoggingEnabled()
				.Build()
				.Solve();

			// TODO Give information tactics?

			Console.WriteLine($"Playing {iterationCount}:");
			Console.WriteLine($"  min     = {result.Min}");
			Console.WriteLine($"  median  = {result.Median}");
			Console.WriteLine($"  average = {result.Average}");
			Console.WriteLine($"  max     = {result.Max}");
		}
	}
}

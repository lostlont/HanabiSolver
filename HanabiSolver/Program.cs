using HanabiSolver.Solver.Builders;
using HanabiSolver.Tactics;
using System;

namespace HanabiSolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int iterationCount = 1000;

			var result = SolverBuilder.New
				.WithIterationCount(iterationCount)
				.WithTactics(new PlayFirst())
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

using HanabiSolver.Factories;
using HanabiSolver.Library.Game;
using HanabiSolver.Tactics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver
{
	public struct Score
	{
		public int Average { get; }
		public int Median { get; }
		public int Min { get; }
		public int Max { get; }

		public Score(int average, int median, int min, int max)
		{
			Average = average;
			Median = median;
			Min = min;
			Max = max;
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			const int amount = 1000;
			var score = Play(amount);
			Console.WriteLine($"Playing {amount}:");
			Console.WriteLine($"  min     = {score.Min}");
			Console.WriteLine($"  median  = {score.Median}");
			Console.WriteLine($"  average = {score.Average}");
			Console.WriteLine($"  max     = {score.Max}");
		}

		private static Score Play(int amount)
		{
			var score = Enumerable
				.Range(0, amount)
				.Select(_ => Play())
				.ToList();

			var average = (int)score.Average();
			var median = Median(score);
			var min = score.Min();
			var max = score.Max();
			return new Score(average, median, min, max);
		}

		private static int Median(IEnumerable<int> source)
		{
			var ordered = source
				.OrderBy(v => v)
				.ToList();
			var centerAverage = (int)Center(ordered).Average();
			return centerAverage;
		}

		private static IEnumerable<TSource> Center<TSource>(IEnumerable<TSource> source)
		{
			if (source.Count() % 2 == 1)
				return source.Skip(source.Count() / 2).Take(1);
			else
				return source.Skip(source.Count() / 2 - 1).Take(2);
		}

		private static int Play()
		{
			var gameManager = GameManagerFactory.Create();
			//Log.Info(gameManager);

			gameManager.Play(new List<ITactics> { new PlayFirst() });
			//Log.Info(gameManager);

			return gameManager.Score;
		}
	}
}

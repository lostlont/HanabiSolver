using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Solver
{
	public struct Result
	{
		public Result(int average, int median, int min, int max)
		{
			Average = average;
			Median = median;
			Min = min;
			Max = max;
		}

		public int Average { get; }
		public int Median { get; }
		public int Min { get; }
		public int Max { get; }

		public static Result FromValues(IEnumerable<int> values)
		{
			var first = values.First();
			var aggregate = values.Skip(1).Aggregate(
				new
				{
					Sum = first,
					Distribution = new Dictionary<int, int> { [first] = 1 },
					Min = first,
					Max = first,
				},
				(previous, current) => new
				{
					Sum = previous.Sum + 1,
					Distribution = AddEntry(previous.Distribution, current),
					Min = Math.Min(previous.Min, current),
					Max = Math.Max(previous.Max, current),
				});

			static Dictionary<int, int> AddEntry(Dictionary<int, int> distribution, int value)
			{
				distribution.TryGetValue(value, out var valueCount);
				distribution[value] = valueCount + 1;
				return distribution;
			}

			var count = aggregate.Distribution.Values.Sum();
			var average = (int)Math.Round((float)aggregate.Sum / count);
			var median = aggregate.Distribution.OrderByDescending(x => x.Value).First().Key;
			var min = aggregate.Min;
			var max = aggregate.Max;
			return new Result(average, median, min, max);
		}
	}
}

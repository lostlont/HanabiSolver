using HanabiSolver.Common.Extensions;
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
			var average = (int)values.Average();
			var median = values.Median();
			var min = values.Min();
			var max = values.Max();
			return new Result(average, median, min, max);
		}
	}
}

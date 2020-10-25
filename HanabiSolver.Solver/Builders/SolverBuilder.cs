using HanabiSolver.Library.Game;
using System.Collections.Generic;

namespace HanabiSolver.Solver.Builders
{
	public class SolverBuilder
	{
		private readonly List<ITactics> tactics = new List<ITactics>();
		private int iterationCount = 1;
		private bool isLoggingEnabled = false;

		public static SolverBuilder New => new SolverBuilder();

		public SolverBuilder WithIterationCount(int iterationCount)
		{
			this.iterationCount = iterationCount;
			return this;
		}

		public SolverBuilder WithTactics(ITactics tactics)
		{
			this.tactics.Add(tactics);
			return this;
		}

		public SolverBuilder WithTactics(params ITactics[] tactics)
		{
			this.tactics.AddRange(tactics);
			return this;
		}

		public SolverBuilder WithLoggingEnabled(bool enabled = true)
		{
			isLoggingEnabled = enabled;
			return this;
		}

		public ISolver Build()
		{
			return new Solver(tactics, iterationCount, isLoggingEnabled);
		}
	}
}

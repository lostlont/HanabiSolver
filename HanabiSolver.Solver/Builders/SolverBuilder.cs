using HanabiSolver.Library.Game;
using System;
using System.Collections.Generic;

namespace HanabiSolver.Solver.Builders
{
	public class SolverBuilder
	{
		private readonly List<Func<ITactics>> tactics = new List<Func<ITactics>>();
		private int iterationCount = 1;
		private ILog log = new Log { Enabled = false };

		public static SolverBuilder New => new SolverBuilder();

		public SolverBuilder WithIterationCount(int iterationCount)
		{
			this.iterationCount = iterationCount;
			return this;
		}

		public SolverBuilder WithTactics(params Func<ITactics>[] tactics)
		{
			this.tactics.AddRange(tactics);
			return this;
		}

		public SolverBuilder WithLogger(ILog log)
		{
			this.log = log;
			return this;
		}

		public ISolver Build()
		{
			return new Solver(tactics, iterationCount, log);
		}
	}
}

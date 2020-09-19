using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class PileBuilder
	{
		public List<Card> Cards { get; set; }

		public PileBuilder(IEnumerable<Card> cards)
		{
			Cards = cards.ToList();
		}

		public Pile Build()
		{
			var result = new Pile();
			Cards.ForEach(result.Add);

			return result;
		}
	}
}

using HanabiSolver.Library.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class PlayerBuilder
	{
		public Func<IEnumerable<Card>> CardsBuilder { get; set; } = () => Enumerable.Empty<Card>();
		public TableBuilder TableBuilder { get; set; } = new TableBuilder();

		public Player Build()
		{
			var cards = CardsBuilder();
			var table = TableBuilder.Build();
			return new Player(cards, table);
		}
	}
}

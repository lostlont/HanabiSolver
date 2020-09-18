using HanabiSolver.Library.Game;
using System.Collections.Generic;

namespace HanabiSolver.Library.Tests.Builders
{
	public class PlayerBuilder
	{
		public IEnumerable<Card> Cards { get; set; }
		public TableBuilder TableBuilder { get; set; }

		public PlayerBuilder(IEnumerable<Card> cards, TableBuilder tableBuilder)
		{
			Cards = cards;
			TableBuilder = tableBuilder;
		}

		public Player Build()
		{
			var table = TableBuilder.Build();
			return new Player(Cards, table);
		}
	}
}

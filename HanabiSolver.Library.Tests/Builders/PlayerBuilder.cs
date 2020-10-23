using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class PlayerBuilder
	{
		public IEnumerable<Card> Cards { get; set; }
		public TableBuilder TableBuilder { get; set; } = new TableBuilder();

		public PlayerBuilder()
		{
			Cards = new List<Card>
			{
				new Card(Suite.White, Number.Five),
				new Card(Suite.White, Number.One),
				new Card(Suite.Red, Number.Three),
			};
		}

		public Player Build()
		{
			var cards = Cards.ToList();
			var table = TableBuilder.Build();
			return new Player(cards, table);
		}
	}
}

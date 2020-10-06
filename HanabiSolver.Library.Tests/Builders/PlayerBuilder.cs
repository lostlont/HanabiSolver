using HanabiSolver.Library.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Tests.Builders
{
	public class PlayerBuilder
	{
		public IEnumerable<Card> Cards { get; set; }
		public TableBuilder TableBuilder { get; set; } = new TableBuilder();
		// TODO Concretize InformationBuilder.
		public Func<Card, Information> InformationBuilder { get; set; } = card => new Information();

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
			var information = cards.ToDictionary(
				card => card,
				InformationBuilder);

			return new Player(cards, table, information);
		}
	}
}

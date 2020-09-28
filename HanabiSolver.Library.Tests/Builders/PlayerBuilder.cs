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
		public Func<IEnumerable<Card>, Dictionary<Card, Information>> InformationBuilder { get; set; } = cards => cards.ToDictionary(card => card, card => new Information());

		public Player Build()
		{
			var cards = CardsBuilder();
			var table = TableBuilder.Build();
			var information = InformationBuilder(cards);
			return new Player(cards, table, information);
		}
	}
}

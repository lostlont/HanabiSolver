using HanabiSolver.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Player
	{
		private readonly List<Card> cards;
		private readonly ITable table;

		public IReadOnlyCollection<Card> Cards => cards;

		public Player(IEnumerable<Card> cards, ITable table)
		{
			this.cards = cards.ToList();
			this.table = table;
		}

		public void Discard(Card card)
		{
			var removed = cards.Remove(card);
			if (!removed)
				throw new InvalidOperationException();

			table.DiscardPile.Add(card);

			var newCard = table.Deck.Draw();
			cards.Insert(0, newCard);
		}
	}
}

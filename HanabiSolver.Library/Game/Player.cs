using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Player
	{
		private readonly List<Card> cards;
		private readonly Deck deck;
		private readonly ICollection<Card> discardPile;

		public IReadOnlyCollection<Card> Cards => cards;

		public Player(IEnumerable<Card> cards, Deck deck, ICollection<Card> discardPile)
		{
			this.cards = cards.ToList();
			this.deck = deck;
			this.discardPile = discardPile;
		}

		public void Discard(Card card)
		{
			var removed = cards.Remove(card);
			if (!removed)
				throw new InvalidOperationException();

			discardPile.Add(card);

			var newCard = deck.Draw();
			cards.Insert(0, newCard);
		}
	}
}

using HanabiSolver.Library.Extensions;
using HanabiSolver.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Player
	{
		private readonly List<Card> cards;

		public IReadOnlyCollection<Card> Cards => cards;
		public ITable Table { get; }

		public Player(IEnumerable<Card> cards, ITable table)
		{
			this.cards = cards.ToList();
			Table = table;
		}

		public void Discard(Card card)
		{
			var removed = cards.Remove(card);
			if (!removed)
				throw new InvalidOperationException();

			Table.DiscardPile.Add(card);

			var newCard = Table.Deck.Draw();
			cards.Insert(0, newCard);

			Table.InformationTokens.Replenish();
		}

		public void Play(Card card)
		{
			var pile = Table.PlayedCards[card.Suite];
			if (CanPlay(card, pile))
			{
				pile.Add(card);

				if (card.Number == Number.Five)
					Table.InformationTokens.Replenish();
			}
			else
			{
				Table.FuseTokens.Replenish();
				Table.DiscardPile.Add(card);
			}
		}

		private bool CanPlay(Card card, Pile pile)
		{
			var lastNumber = pile.Cards.LastOrDefault()?.Number;
			var expectedNextNumber = lastNumber?.Next() ?? Number.One;

			return card.Number == expectedNextNumber;
		}
	}
}

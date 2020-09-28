using HanabiSolver.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Player
	{
		private readonly List<Card> cards;

		public IReadOnlyCollection<Card> Cards => cards;
		public Table Table { get; }

		public Player(IEnumerable<Card> cards, Table table)
		{
			this.cards = cards.ToList();
			Table = table;
		}

		public void Discard(Card card)
		{
			RemoveCard(card);
			Table.DiscardPile.Add(card);

			var newCard = Table.Deck.Draw();
			cards.Insert(0, newCard);

			Table.InformationTokens.Replenish();
		}

		public void Play(Card card)
		{
			RemoveCard(card);

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

			var newCard = Table.Deck.Draw();
			cards.Insert(0, newCard);
		}

		private void RemoveCard(Card card)
		{
			var removed = cards.Remove(card);
			if (!removed)
				throw new InvalidOperationException();
		}

		private bool CanPlay(Card card, Pile pile)
		{
			var lastNumber = pile.Cards.LastOrDefault()?.Number;
			var expectedNextNumber = lastNumber?.Next() ?? Number.One;

			return card.Number == expectedNextNumber;
		}
	}
}

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

			Table.Tokens.Replenish();
		}

		public void Play(Card card)
		{
			var pile = Table.PlayedCards[card.Suite];
			if ((card.Number == Number.One && pile.Cards.None()) || (card.Number == pile.Cards.Last().Number.Next())) // TODO Refactor
			{
				pile.Add(card);
			}
			else
			{
				Table.FuseTokens.Replenish();
				Table.DiscardPile.Add(card);
			}
		}
	}
}

using HanabiSolver.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Player
	{
		private readonly List<Card> cards;
		private readonly Dictionary<Card, Information> information = new Dictionary<Card, Information>();

		public IReadOnlyCollection<Card> Cards => cards;
		public IReadOnlyDictionary<Card, Information> Information => information;
		public Table Table { get; }

		public Player(IEnumerable<Card> cards, Table table)
			: this(cards, table, CreateInformation(cards))
		{
		}

		public Player(IEnumerable<Card> cards, Table table, IReadOnlyDictionary<Card, Information> information)
		{
			this.cards = cards.ToList();
			this.information = information.ToDictionary(e => e.Key, e => e.Value);
			Table = table;
		}

		private static Dictionary<Card, Information> CreateInformation(IEnumerable<Card> cards)
		{
			return cards.ToDictionary(c => c, _ => new Information());
		}

		public void Discard(Card card)
		{
			RemoveCard(card);
			Table.DiscardPile.Add(card);

			DrawCard();

			Table.InformationTokens.Replenish();
		}

		public void GiveInformation(Player otherPlayer, Suite suite)
		{
			if (Table.InformationTokens.Amount <= 0)
				throw new InvalidOperationException();

			var informedCards = otherPlayer.InformationAffectedCards(suite);

			if (informedCards.None())
				throw new InvalidOperationException();

			Table.InformationTokens.Use();
			foreach (var information in informedCards.Select(c => otherPlayer.Information[c]))
				information.IsSuiteKnown = true;
		}

		public void GiveInformation(Player otherPlayer, Number number)
		{
			if (Table.InformationTokens.Amount <= 0)
				throw new InvalidOperationException();

			var informedCards = otherPlayer.InformationAffectedCards(number);

			if (informedCards.None())
				throw new InvalidOperationException();

			Table.InformationTokens.Use();
			foreach (var information in informedCards.Select(c => otherPlayer.Information[c]))
				information.IsNumberKnown = true;
		}

		public bool CanGiveInformation(Player otherPlayer, Suite suite)
		{
			return (Table.InformationTokens.Amount > 0)
				&& otherPlayer.InformationAffectedCards(suite).Any();
		}

		public bool CanGiveInformation(Player otherPlayer, Number number)
		{
			return (Table.InformationTokens.Amount > 0)
				&& otherPlayer.InformationAffectedCards(number).Any();
		}

		public IEnumerable<Card> InformationAffectedCards(Suite suite)
		{
			return Cards
				.Where(c => c.Suite == suite)
				.Where(c => Information[c].IsSuiteKnown == false);
		}

		public IEnumerable<Card> InformationAffectedCards(Number number)
		{
			return Cards
				.Where(c => c.Number == number)
				.Where(c => Information[c].IsNumberKnown == false);
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

			DrawCard();
		}

		private void DrawCard()
		{
			var newCard = Table.Deck.Draw();
			cards.Insert(0, newCard);

			information[newCard] = new Information();
		}

		private void RemoveCard(Card card)
		{
			var removed = cards.Remove(card);
			if (!removed)
				throw new InvalidOperationException();

			information.Remove(card);
		}

		private bool CanPlay(Card card, IPile pile)
		{
			var expectedNextNumber = pile.Top?.Number.Next() ?? Number.One;

			return card.Number == expectedNextNumber;
		}
	}
}

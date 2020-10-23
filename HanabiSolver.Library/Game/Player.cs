using HanabiSolver.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IPlayer
	{
		IReadOnlyList<Card> Cards { get; }
		IReadOnlyDictionary<Card, Information> Information { get; }
	}

	public class Player : IPlayer
	{
		private readonly List<Card> cards;
		private readonly Dictionary<Card, Information> information = new Dictionary<Card, Information>();

		public IReadOnlyList<Card> Cards => cards;
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

			var informedCards = InformationAffectedCards(otherPlayer, suite);

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

			var informedCards = InformationAffectedCards(otherPlayer, number);

			if (informedCards.None())
				throw new InvalidOperationException();

			Table.InformationTokens.Use();
			foreach (var information in informedCards.Select(c => otherPlayer.Information[c]))
				information.IsNumberKnown = true;
		}

		public bool CanGiveInformation(IPlayer otherPlayer, Suite suite)
		{
			return (Table.InformationTokens.Amount > 0)
				&& InformationAffectedCards(otherPlayer, suite).Any();
		}

		public bool CanGiveInformation(IPlayer otherPlayer, Number number)
		{
			return (Table.InformationTokens.Amount > 0)
				&& InformationAffectedCards(otherPlayer, number).Any();
		}

		private IEnumerable<Card> InformationAffectedCards(IPlayer otherPlayer, Suite suite)
		{
			return otherPlayer.Cards
				.Where(c => c.Suite == suite)
				.Where(c => otherPlayer.Information[c].IsSuiteKnown == false);
		}

		private IEnumerable<Card> InformationAffectedCards(IPlayer otherPlayer, Number number)
		{
			return otherPlayer.Cards
				.Where(c => c.Number == number)
				.Where(c => otherPlayer.Information[c].IsNumberKnown == false);
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

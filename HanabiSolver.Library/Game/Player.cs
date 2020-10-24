using HanabiSolver.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IReadOnlyPlayer
	{
		IReadOnlyList<Card> Cards { get; }
		IReadOnlyDictionary<Card, IReadOnlyInformation> Information { get; }

		IEnumerable<Card> InformationAffectedCards(Suite suite);
		IEnumerable<Card> InformationAffectedCards(Number number);
	}

	public interface IInformationReceiver
	{
		void ReceiveInformation(Suite suite);
		void ReceiveInformation(Number number);
	}

	public interface IInformationReceiverReadOnlyPlayer : IReadOnlyPlayer, IInformationReceiver
	{
	}


	public interface IPlayer : IInformationReceiverReadOnlyPlayer
	{
		void Discard(Card card);
		void GiveInformation(IInformationReceiverReadOnlyPlayer otherPlayer, Suite suite);
		void GiveInformation(IInformationReceiverReadOnlyPlayer otherPlayer, Number number);
		bool CanGiveInformation(IReadOnlyPlayer otherPlayer, Suite suite);
		bool CanGiveInformation(IReadOnlyPlayer otherPlayer, Number number);
		void Play(Card card);
	}

	public class Player : IPlayer
	{
		private readonly List<Card> cards;
		private readonly Dictionary<Card, Information> information = new Dictionary<Card, Information>();

		public IReadOnlyList<Card> Cards => cards;
		public IReadOnlyDictionary<Card, Information> Information => information;
		public Table Table { get; }

		IReadOnlyDictionary<Card, IReadOnlyInformation> IReadOnlyPlayer.Information => Information.ToDictionary(e => e.Key, e => (IReadOnlyInformation)e.Value);

		public Player(IEnumerable<Card> cards, Table table)
		{
			this.cards = cards.ToList();
			information = cards.ToDictionary(c => c, _ => new Information());
			Table = table;
		}

		public void Discard(Card card)
		{
			RemoveCard(card);
			Table.DiscardPile.Add(card);

			DrawCard();

			Table.InformationTokens.Replenish();
		}

		public void GiveInformation(IInformationReceiverReadOnlyPlayer otherPlayer, Suite suite)
		{
			if (Table.InformationTokens.Amount <= 0)
				throw new InvalidOperationException();

			Table.InformationTokens.Use();
			otherPlayer.ReceiveInformation(suite);
		}

		public void GiveInformation(IInformationReceiverReadOnlyPlayer otherPlayer, Number number)
		{
			if (Table.InformationTokens.Amount <= 0)
				throw new InvalidOperationException();

			Table.InformationTokens.Use();
			otherPlayer.ReceiveInformation(number);
		}

		public bool CanGiveInformation(IReadOnlyPlayer otherPlayer, Suite suite)
		{
			return (Table.InformationTokens.Amount > 0)
				&& otherPlayer.InformationAffectedCards(suite).Any();
		}

		public bool CanGiveInformation(IReadOnlyPlayer otherPlayer, Number number)
		{
			return (Table.InformationTokens.Amount > 0)
				&& otherPlayer.InformationAffectedCards(number).Any();
		}

		public void ReceiveInformation(Suite suite)
		{
			var informedCards = InformationAffectedCards(suite);
			if (informedCards.None())
				throw new InvalidOperationException();

			foreach (var information in informedCards.Select(c => Information[c]))
				information.IsSuiteKnown = true;
		}

		public void ReceiveInformation(Number number)
		{
			var informedCards = InformationAffectedCards(number);
			if (informedCards.None())
				throw new InvalidOperationException();

			foreach (var information in informedCards.Select(c => Information[c]))
				information.IsNumberKnown = true;
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
			if (newCard != null)
			{
				cards.Insert(0, newCard);

				information[newCard] = new Information();
			}
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

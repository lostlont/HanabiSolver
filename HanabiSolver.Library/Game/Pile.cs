using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IPile
	{
		IReadOnlyCollection<Card> Cards { get; }
		Card? Top { get; }

		void Add(Card card);
	}

	public class Pile : IPile
	{
		private readonly List<Card> cards;

		public Pile()
			: this(Enumerable.Empty<Card>())
		{
		}

		public Pile(IEnumerable<Card> cards)
		{
			this.cards = cards.ToList();
		}

		public IReadOnlyCollection<Card> Cards => cards;

		public Card? Top => cards.LastOrDefault();

		public void Add(Card card)
		{
			cards.Add(card);
		}
	}
}

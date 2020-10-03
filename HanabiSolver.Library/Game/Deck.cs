using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IDeck
	{
		IReadOnlyCollection<Card> Cards { get; }
		Card Top { get; }

		Card Draw();
	}

	public partial class Deck : IDeck
	{
		private readonly Queue<Card> cards;

		public IReadOnlyCollection<Card> Cards => cards;

		public Card Top => cards.Peek();

		public Deck(IEnumerable<Card> cards)
		{
			this.cards = new Queue<Card>(cards);
		}

		public Card Draw()
		{
			return cards.Dequeue();
		}

		public bool CanDraw() => cards.Any();
	}
}

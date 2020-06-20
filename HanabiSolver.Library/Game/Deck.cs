using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public partial class Deck
	{
		private readonly Queue<Card> cards;

		public IReadOnlyCollection<Card> Cards => cards;

		public Deck(IEnumerable<Card> cards)
		{
			this.cards = new Queue<Card>(cards);
		}

		public Card? Draw()
		{
			cards.TryDequeue(out var result);
			return result;
		}

		public bool CanDraw() => cards.Any();
	}
}

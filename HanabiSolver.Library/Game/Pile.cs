using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class Pile
	{
		private readonly List<Card> cards = new List<Card>();

		public IReadOnlyCollection<Card> Cards => cards;

		public Card? Top => cards.LastOrDefault();
		// TODO Directly test Top.

		public void Add(Card card)
		{
			cards.Add(card);
		}
	}
}

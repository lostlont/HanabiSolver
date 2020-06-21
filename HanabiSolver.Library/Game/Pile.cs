using System.Collections.Generic;

namespace HanabiSolver.Library.Game
{
	public class Pile
	{
		private readonly List<Card> cards = new List<Card>();

		public IReadOnlyCollection<Card> Cards => cards;

		public void Add(Card card)
		{
			cards.Add(card);
		}
	}
}

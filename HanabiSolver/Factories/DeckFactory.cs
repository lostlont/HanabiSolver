using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Factories
{
	public static class DeckFactory
	{
		public static IDeck CreateDeck()
		{
			var random = new Random();
			var deck = new Deck(GenerateCards().OrderBy(c => random.Next()));
			return deck;
		}

		private static IEnumerable<Card> GenerateCards()
		{
			foreach (var suite in EnumUtils.Values<Suite>())
				foreach (var number in EnumUtils.Values<Number>())
					foreach (var instance in Enumerable.Range(0, AmountOf(number)))
						yield return new Card(suite, number);
		}

		private static int AmountOf(Number number)
		{
			return number switch
			{
				Number.One => 3,
				Number.Two => 2,
				Number.Three => 2,
				Number.Four => 2,
				Number.Five => 1,
				_ => throw new InvalidOperationException(),
			};
		}

	}
}

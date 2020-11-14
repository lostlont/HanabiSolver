using HanabiSolver.Library.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver
{
	public static class Utils
	{
		public static bool IsKnownCard(IReadOnlyPlayer player, Card card)
			=> IsKnown(player.Information[card]);

		public static bool IsKnown(IReadOnlyInformation information)
			=> information.IsSuiteKnown && information.IsNumberKnown;

		public static bool IsUnknownCard(IReadOnlyPlayer player, Card card)
			=> IsUnknown(player.Information[card]);

		public static bool IsUnknown(IReadOnlyInformation information)
			=> !information.IsSuiteKnown && !information.IsNumberKnown;

		public static int AmountOf(Number number)
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

		public static int DiscardedSimilarCount(IGameState gameState, Card card)
		{
			return SimilarCount(gameState.Table.DiscardPile.Cards, card);
		}

		public static int PlayedSimilarCount(IGameState gameState, Card card)
		{
			return SimilarCount(gameState.Table.PlayedCards[card.Suite].Cards, card);
		}

		public static int SimilarCount(IEnumerable<Card> cards, Card card)
		{
			return cards
				.Where(c => Equals(c, card))
				.Count();
		}

		public static bool Equals(Card left, Card right)
		{
			return
				(left.Suite == right.Suite) &&
				(left.Number == right.Number);
		}
	}
}

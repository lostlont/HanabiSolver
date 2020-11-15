using HanabiSolver.Library.Game;

namespace HanabiSolver.Extensions
{
	public static class PlayerExtensions
	{
		public static int CardNumber(this IReadOnlyPlayer player, Card card)
		{
			return player.Cards.IndexOf(card) + 1;
		}
	}
}

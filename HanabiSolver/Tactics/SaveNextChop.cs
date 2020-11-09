using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class SaveNextChop : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			var hasInformationToken = gameState.Table.InformationTokens.Amount > 0;
			var chop = Chop(gameState.NextPlayer);
			var hasChop = chop != null;
			if (hasChop)
			{
				var discardedSimilarCount = gameState.Table.DiscardPile.Cards
					.Where(c => Equals(c, chop!))
					.Count();

				var playedSimilarCount = gameState.Table.PlayedCards[chop!.Suite].Cards
					.Where(c => Equals(c, chop!))
					.Count();

				var savedSimilarCount = gameState.Players
					.Where(p => p != gameState.CurrentPlayer)
					.SelectMany(p => p.Cards.Where(c => Equals(c, chop!)).Where(c => !Utils.IsUnknownCard(p, c)))
					.Count();
			}

			return hasInformationToken && hasChop;
		}

		private bool IsChopInDanger(IGameState gameState)
		{
			var chop = Chop(gameState.NextPlayer)!;

			// TODO This is more complex...
			var usedCount =
				Utils.DiscardedSimilarCount(gameState, chop) +
				Utils.PlayedSimilarCount(gameState, chop) +
				Utils.SavedSimilarCount(gameState, chop);

			return false;
			//return usedCount Utils.AmountOf(chop.Number);
		}

		private bool Equals(Card left, Card right)
		{
			return
				(left.Suite == right.Suite) &&
				(left.Number == right.Number);
		}

		public void Apply(IGameState gameState)
		{
			var chop = Chop(gameState.NextPlayer)!;

			if (IsSuiteBetterInformation(gameState.NextPlayer, chop))
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Suite);
			else
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Number);
		}

		private bool IsSuiteBetterInformation(IReadOnlyPlayer nextPlayer, Card chop)
		{
			var suiteInformationAffectedCards = nextPlayer.InformationAffectedCards(chop.Suite);
			var numberInformationAffectedCards = nextPlayer.InformationAffectedCards(chop.Number);

			if (suiteInformationAffectedCards.Count() == 1)
				return true;
			else if (numberInformationAffectedCards.Count() == 1)
				return false;
			else
				return suiteInformationAffectedCards.Count() > numberInformationAffectedCards.Count();
		}

		private bool HasChop(IReadOnlyPlayer player) => Chop(player) != null;

		private Card? Chop(IReadOnlyPlayer player)
		{
			return player.Cards
				.Reverse()
				.FirstOrDefault(c => Utils.IsUnknownCard(player, c));
		}
	}
}

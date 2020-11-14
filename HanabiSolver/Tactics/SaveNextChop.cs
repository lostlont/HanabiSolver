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

			return hasInformationToken && (chop != null) && IsCardInDanger(gameState, chop);
		}

		public void Apply(IGameState gameState)
		{
			var chop = Chop(gameState.NextPlayer)!;

			if (IsSuiteBetterInformation(gameState.NextPlayer, chop))
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Suite);
			else
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Number);
		}

		private Card? Chop(IReadOnlyPlayer player)
		{
			return player.Cards
				.Reverse()
				.FirstOrDefault(c => Utils.IsUnknownCard(player, c));
		}

		private bool IsCardInDanger(IGameState gameState, Card card)
		{
			var noSimilarPlayed = Utils.PlayedSimilarCount(gameState, card) == 0;
			var allOtherSimilarDiscarded = Utils.DiscardedSimilarCount(gameState, card) == (Utils.AmountOf(card.Number) - 1);

			return noSimilarPlayed && allOtherSimilarDiscarded;
		}

		private bool IsSuiteBetterInformation(IReadOnlyPlayer nextPlayer, Card chop)
		{
			var suiteInformationAffectedCards = nextPlayer.InformationAffectedCards(chop.Suite);
			var numberInformationAffectedCards = nextPlayer.InformationAffectedCards(chop.Number);

			return suiteInformationAffectedCards.Count() <= numberInformationAffectedCards.Count();

			/*
			if (suiteInformationAffectedCards.Count() == 1)
				return true;
			else if (numberInformationAffectedCards.Count() == 1)
				return false;
			else
				return suiteInformationAffectedCards.Count() > numberInformationAffectedCards.Count();
			*/
		}
	}
}

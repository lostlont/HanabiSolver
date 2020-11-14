using HanabiSolver.Common.Extensions;
using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class CluePlayable : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			var hasInformationTokens = gameState.Table.InformationTokens.Amount > 0;
			var hasApplicableCard = FirstApplicableCard(gameState) != null;

			return hasInformationTokens && hasApplicableCard;
		}

		public void Apply(IGameState gameState)
		{
			var card = FirstApplicableCard(gameState)!;

			if (gameState.NextPlayer.Information[card].IsSuiteKnown)
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, card.Number);
			else
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, card.Suite);
		}

		private Card? FirstApplicableCard(IGameState gameState)
		{
			return gameState.NextPlayer.Cards
				.Where(c => IsCluable(gameState.NextPlayer, c))
				.Where(c => IsPlayable(gameState, c))
				.FirstOrDefault();
		}

		private bool IsCluable(IReadOnlyPlayer player, Card card)
		{
			return !Utils.IsKnownCard(player, card);
		}

		private bool IsPlayable(IGameState gameState, Card card)
		{
			return card.Number == gameState.Table.PlayedCards[card.Suite].Top?.Number.Next();
		}
	}
}

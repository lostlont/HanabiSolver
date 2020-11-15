using HanabiSolver.Common.Extensions;
using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class CluePlayable : ITactics
	{
		public CluePlayable(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			var hasInformationTokens = gameState.Table.InformationTokens.Amount > 0;
			var hasApplicableCard = FirstApplicableCard(gameState) != null;

			return hasInformationTokens && hasApplicableCard;
		}

		public void Apply(IGameState gameState)
		{
			var card = FirstApplicableCard(gameState)!;

			var cardNumber = gameState.NextPlayer.CardNumber(card);
			var currentPlayerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			var nextPlayerNumber = gameState.PlayerNumber(gameState.NextPlayer);
			Log.Write($"Player {currentPlayerNumber} applies {nameof(CluePlayable)} because card ");
			Log.Info(card);
			Log.WriteLine($", card {cardNumber} of player {nextPlayerNumber} is not known but would be playable.");

			if (gameState.NextPlayer.Information[card].IsSuiteKnown)
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, card.Number);
			else
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, card.Suite);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
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
			return card.Number == (gameState.Table.PlayedCards[card.Suite].Top?.Number.Next() ?? Number.One);
		}
	}
}

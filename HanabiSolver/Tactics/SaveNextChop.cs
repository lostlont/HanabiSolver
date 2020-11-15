using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class SaveNextChop : ITactics
	{
		public SaveNextChop(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			var hasInformationToken = gameState.Table.InformationTokens.Amount > 0;
			var chop = Chop(gameState.NextPlayer);

			return hasInformationToken && (chop != null) && IsCardInDanger(gameState, chop);
		}

		public void Apply(IGameState gameState)
		{
			var chop = Chop(gameState.NextPlayer)!;

			var cardNumber = gameState.NextPlayer.CardNumber(chop);
			var currentPlayerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			var nextPlayerNumber = gameState.PlayerNumber(gameState.NextPlayer);
			Log.Write($"Player {currentPlayerNumber} applies {nameof(SaveNextChop)} because chop ");
			Log.Info(chop);
			Log.WriteLine($", card {cardNumber} of player {nextPlayerNumber} is in danger.");

			var top = gameState.Table.Deck.Top;
			if (top != null)
			{
				Log.Write($"  draws card ");
				Log.Info(top);
				Log.WriteLine();
			}

			if (IsSuiteBetterInformation(gameState.NextPlayer, chop))
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Suite);
			else
				gameState.CurrentPlayer.GiveInformation(gameState.NextPlayer, chop.Number);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
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

			// TODO SaveNextChop may give the better clue if other tactics, e.g. PlayStandaloneClued does not pick them up eagerly.
		}
	}
}

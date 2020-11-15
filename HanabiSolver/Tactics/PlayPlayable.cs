using HanabiSolver.Common.Extensions;
using HanabiSolver.Extensions;
using HanabiSolver.Library.Game;
using HanabiSolver.Solver;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayPlayable : ITactics
	{
		public PlayPlayable(Log log)
		{
			Log = log;
		}

		public Log Log { get; }

		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any(c => IsPlayable(gameState, c));
		}

		public void Apply(IGameState gameState)
		{
			var playableCard = gameState.CurrentPlayer.Cards.First(c => IsPlayable(gameState, c));

			var cardNumber = gameState.CurrentPlayer.CardNumber(playableCard);
			var playerNumber = gameState.PlayerNumber(gameState.CurrentPlayer);
			Log.Write($"Player {playerNumber} applies {nameof(PlayPlayable)} because card ");
			Log.Info(playableCard);
			Log.WriteLine($", card {cardNumber} is playable.");

			var top = gameState.Table.Deck.Top;
			if (top != null)
			{
				Log.Write($"  draws card ");
				Log.Info(top);
				Log.WriteLine();
			}

			gameState.CurrentPlayer.Play(playableCard);

			Log.WriteLine();
			Log.Info(gameState);
			Log.WriteLine();
		}

		private bool IsPlayable(IGameState gameState, Card card)
		{
			var isKnown = Utils.IsKnownCard(gameState.CurrentPlayer, card);
			var isNext = card.Number == gameState.Table.PlayedCards[card.Suite].Top?.Number.Next();

			return isKnown && isNext;
		}
	}
}

using HanabiSolver.Library.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public class GameManager
	{
		private IPlayer? lastPlayer = null;

		public GameManager(GameState gameState)
		{
			GameState = gameState;
		}

		public GameState GameState { get; }
		public bool IsEnded
		{
			get
			{
				return
					(GameState.Table.FuseTokens.Amount >= GameState.Table.FuseTokens.MaxAmount) ||
					GameState.Table.PlayedCards.Values.All(pile => pile.Top?.Number == Number.Five) ||
					(GameState.CurrentPlayer == lastPlayer);
			}
		}

		public void Play(IEnumerable<ITactics> tactics)
		{
			while (!IsEnded)
			{
				tactics
					.First(t => t.CanApply(GameState))
					.Apply(GameState);

				if ((lastPlayer == null) && GameState.Table.Deck.Cards.None())
					lastPlayer = GameState.CurrentPlayer;

				GameState.CurrentPlayer = GameState.NextPlayer;
			}
		}
	}
}

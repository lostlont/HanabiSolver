using HanabiSolver.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

		public int Score => GameState.Table.PlayedCards.Values
			.Select(p => p.Top)
			.OfType<Card>()
			.Select(c => c.Number)
			.Select(ValueOf)
			.Sum();

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

		private int ValueOf(Number number)
		{
			return number switch
			{
				Number.One => 1,
				Number.Two => 2,
				Number.Three => 3,
				Number.Four => 4,
				Number.Five => 5,
				_ => throw new SwitchExpressionException(number),
			};
		}
	}
}

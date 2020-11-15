using HanabiSolver.Library.Game;
using System;
using System.Linq;

namespace HanabiSolver.Solver
{
	public interface ILog
	{
		void Write(string message);
		void WriteLine();
		void WriteLine(string message);
		void Info(GameManager gameManager);
		void Info(IGameState gameState);
		void Info(Card card);
	}

	public class Log : ILog
	{
		public bool Enabled { get; set; } = true;

		public void Write(string message)
		{
			if (Enabled)
				Console.Write(message);
		}

		public void WriteLine()
		{
			if (Enabled)
				Console.WriteLine();
		}

		public void WriteLine(string message)
		{
			if (Enabled)
				Console.WriteLine(message);
		}

		public void Info(GameManager gameManager)
		{
			Info(gameManager.GameState);
			WriteLine($"Score: {gameManager.Score}");
			WriteLine();
		}

		public void Info(IGameState gameState)
		{
			WriteLine($"Deck with {gameState.Table.Deck.Cards.Count} cards");
			WriteLine($"Information tokens are {gameState.Table.InformationTokens.Amount}/{gameState.Table.InformationTokens.MaxAmount}");
			WriteLine($"Fuse tokens are {gameState.Table.FuseTokens.Amount}/{gameState.Table.FuseTokens.MaxAmount}");
			WriteLine("Played cards:");
			foreach (var suite in gameState.Table.PlayedCards.Keys)
			{
				var pile = gameState.Table.PlayedCards[suite];
				Write($"  {suite}:");
				foreach (var card in pile.Cards)
				{
					Write(" ");
					Info(card);
				}
				WriteLine();
			}

			foreach (var index in Enumerable.Range(0, gameState.Players.Count))
			{
				var player = gameState.Players[index];
				Info(player, index + 1);
			}
		}

		private void Info(IReadOnlyPlayer player, int number)
		{
			Write($"Player {number}:");
			foreach (var card in player.Cards)
			{
				Write(" ");
				Info(card);
				Write(InformationText(player.Information[card]));
			}
			WriteLine();
		}

		public void Info(Card card)
		{
			if (Enabled)
			{
				Console.ForegroundColor = ColorOf(card.Suite);
				Console.Write((int)card.Number + 1);
				Console.ResetColor();
			}
		}

		private static ConsoleColor ColorOf(Suite suite)
		{
			return suite switch
			{
				Suite.White => ConsoleColor.White,
				Suite.Yellow => ConsoleColor.Yellow,
				Suite.Green => ConsoleColor.DarkGreen,
				Suite.Blue => ConsoleColor.Blue,
				Suite.Red => ConsoleColor.DarkRed,
				_ => throw new InvalidOperationException(),
			};
		}

		private static string InformationText(IReadOnlyInformation information)
		{
			if (information.IsSuiteKnown && information.IsNumberKnown)
				return "(*)";
			else if (information.IsSuiteKnown)
				return "(S)";
			else if (information.IsNumberKnown)
				return "(N)";
			else
				return string.Empty;
		}
	}
}

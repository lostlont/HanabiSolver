using HanabiSolver.Library.Game;
using System;
using System.Linq;

namespace HanabiSolver.Solver
{
	internal static class Log
	{
		public static void Info(GameManager gameManager)
		{
			Info(gameManager.GameState);
			foreach (var index in Enumerable.Range(0, gameManager.GameState.Players.Count))
			{
				var player = gameManager.GameState.Players[index];
				Info(player, index + 1);
			}
			Console.WriteLine($"Score: {gameManager.Score}");
			Console.WriteLine();
		}

		private static void Info(IGameState gameState)
		{
			Console.WriteLine($"Deck with {gameState.Table.Deck.Cards.Count} cards");
			Console.WriteLine($"Information tokens are {gameState.Table.InformationTokens.Amount}/{gameState.Table.InformationTokens.MaxAmount}");
			Console.WriteLine($"Fuse tokens are {gameState.Table.FuseTokens.Amount}/{gameState.Table.FuseTokens.MaxAmount}");
			Console.WriteLine("Played cards:");
			foreach (var suite in gameState.Table.PlayedCards.Keys)
			{
				var pile = gameState.Table.PlayedCards[suite];
				Console.Write($"  {suite}:");
				foreach (var card in pile.Cards)
				{
					Console.Write(" ");
					Info(card);
				}
				Console.WriteLine();
			}
		}

		private static void Info(IReadOnlyPlayer player, int number)
		{
			Console.Write($"Player {number}:");
			foreach (var card in player.Cards)
			{
				Console.Write(" ");
				Info(card);
				Console.Write(InformationText(player.Information[card]));
			}
			Console.WriteLine();
		}

		private static void Info(Card card)
		{
			Console.ForegroundColor = ColorOf(card.Suite);
			Console.Write((int)card.Number + 1);
			Console.ResetColor();
		}

		private static ConsoleColor ColorOf(Suite suite)
		{
			return suite switch
			{
				Suite.White => ConsoleColor.White,
				Suite.Yellow => ConsoleColor.DarkYellow,
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

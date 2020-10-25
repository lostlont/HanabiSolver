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
			foreach (var player in gameManager.GameState.Players)
				Info(player);
			Console.WriteLine($"Score: {gameManager.Score}");
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
				var cards = string.Join(", ", pile.Cards);
				Console.WriteLine($"  {suite}: {cards}");
			}
		}

		private static void Info(IReadOnlyPlayer player)
		{
			Console.WriteLine("Player:");
			var cardsWithInformation = player.Cards.Select(card => $"{card}{InformationText(player.Information[card])}");
			var cards = string.Join(", ", cardsWithInformation);
			Console.WriteLine($"  Cards: {cards}");
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

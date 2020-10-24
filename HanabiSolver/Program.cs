using HanabiSolver.Library.Game;
using HanabiSolver.Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver
{
	public class PlayFirst : ITactics
	{
		public bool CanApply(IGameState gameState)
		{
			return gameState.CurrentPlayer.Cards.Any();
		}

		public void Apply(IGameState gameState)
		{
			gameState.CurrentPlayer.Play(gameState.CurrentPlayer.Cards.First());
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var random = new Random();
			var deck = new Deck(GenerateCards().OrderBy(c => random.Next()));
			var discardPile = new Pile();
			var informationTokens = new Tokens(8, 8);
			var fuseTokens = new Tokens(3, 0);
			var playedCards = EnumUtils.Values<Suite>().ToDictionary(suite => suite, suite => (IPile)new Pile());
			var table = new Table(deck, discardPile, informationTokens, fuseTokens, playedCards);
			var players = Enumerable
				.Range(0, 3)
				.Select(_ => new Player(DrawCards(deck, 5), table))
				.ToList();
			var gameState = new GameState(table, players);
			var gameManager = new GameManager(gameState);

			Log(gameState);
			foreach (var player in players)
				Log(player);

			gameManager.Play(new List<ITactics> { new PlayFirst() });

			Log(gameState);
			foreach (var player in players)
				Log(player);
		}

		private static IEnumerable<Card> GenerateCards()
		{
			foreach (var suite in EnumUtils.Values<Suite>())
				foreach (var number in EnumUtils.Values<Number>())
					foreach (var instance in Enumerable.Range(0, AmountOf(number)))
						yield return new Card(suite, number);
		}

		private static int AmountOf(Number number)
		{
			var result = number switch
			{
				Number.One => 3,
				Number.Two => 2,
				Number.Three => 2,
				Number.Four => 2,
				Number.Five => 1,
				_ => throw new InvalidOperationException(),
			};
			return result;
		}

		private static IEnumerable<Card> DrawCards(IDeck deck, int amount)
		{
			var result = new List<Card>();
			foreach (var cardIndex in Enumerable.Range(0, amount))
			{
				var card = deck.Draw();
				if (card != null)
					result.Add(card);
			}
			return result;
		}

		private static void Log(IGameState gameState)
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

		private static void Log(IReadOnlyPlayer player)
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

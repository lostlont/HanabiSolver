using HanabiSolver.Common.Utils;
using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Solver.Factories
{
	internal static class GameManagerFactory
	{
		public static GameManager Create()
		{
			var deck = DeckFactory.CreateDeck();
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
			return gameManager;
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
	}
}

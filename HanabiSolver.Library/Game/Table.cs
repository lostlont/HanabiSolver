using HanabiSolver.Library.Interfaces;
using System.Collections.Generic;

namespace HanabiSolver.Library.Game
{
	public class Table : ITable
	{
		public Deck Deck { get; }
		public Pile DiscardPile { get; }
		public Tokens Tokens { get; }
		public Tokens FuseTokens { get; }
		public Dictionary<Suite, Pile> PlayedCards { get; }

		public Table(Deck deck, Pile discardPile, Tokens tokens, Tokens fuseTokens, Dictionary<Suite, Pile> playedCards)
		{
			Deck = deck;
			DiscardPile = discardPile;
			Tokens = tokens;
			FuseTokens = fuseTokens;
			PlayedCards = playedCards;
		}
	}
}

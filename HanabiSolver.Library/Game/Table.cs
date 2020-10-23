using System.Collections.Generic;

namespace HanabiSolver.Library.Game
{
	public class Table
	{
		public IDeck Deck { get; }
		public IPile DiscardPile { get; }
		public ITokens InformationTokens { get; }
		public ITokens FuseTokens { get; }
		public IReadOnlyDictionary<Suite, IPile> PlayedCards { get; }

		public Table(IDeck deck, IPile discardPile, ITokens informationTokens, ITokens fuseTokens, Dictionary<Suite, IPile> playedCards)
		{
			Deck = deck;
			DiscardPile = discardPile;
			InformationTokens = informationTokens;
			FuseTokens = fuseTokens;
			PlayedCards = playedCards;
		}
	}
}

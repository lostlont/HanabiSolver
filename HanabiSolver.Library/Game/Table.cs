using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver.Library.Game
{
	public interface IReadOnlyTable
	{
		IReadOnlyDeck Deck { get; }
		IReadOnlyPile DiscardPile { get; }
		IReadOnlyTokens InformationTokens { get; }
		IReadOnlyTokens FuseTokens { get; }
		IReadOnlyDictionary<Suite, IReadOnlyPile> PlayedCards { get; }
	}

	public class Table : IReadOnlyTable
	{
		public IDeck Deck { get; }
		public IPile DiscardPile { get; }
		public ITokens InformationTokens { get; }
		public ITokens FuseTokens { get; }
		public IReadOnlyDictionary<Suite, IPile> PlayedCards { get; }

		IReadOnlyDeck IReadOnlyTable.Deck => Deck;
		IReadOnlyPile IReadOnlyTable.DiscardPile => DiscardPile;
		IReadOnlyTokens IReadOnlyTable.InformationTokens => InformationTokens;
		IReadOnlyTokens IReadOnlyTable.FuseTokens => FuseTokens;
		IReadOnlyDictionary<Suite, IReadOnlyPile> IReadOnlyTable.PlayedCards => PlayedCards.ToDictionary(e => e.Key, e => (IReadOnlyPile)e.Value);

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

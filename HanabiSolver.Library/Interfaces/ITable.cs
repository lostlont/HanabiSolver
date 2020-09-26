using HanabiSolver.Library.Game;
using System.Collections.Generic;

namespace HanabiSolver.Library.Interfaces
{
	public interface ITable
	{
		Deck Deck { get; }
		Pile DiscardPile { get; }
		Tokens InformationTokens { get; }
		Tokens FuseTokens { get; }
		Dictionary<Suite, Pile> PlayedCards { get; }
	}
}

using HanabiSolver.Library.Game;

namespace HanabiSolver.Library.Interfaces
{
	public interface ITable
	{
		Deck Deck { get; }
		Pile DiscardPile { get; }
		Tokens Tokens { get; }
	}
}

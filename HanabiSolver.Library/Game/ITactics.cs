namespace HanabiSolver.Library.Game
{
	public interface ITactics
	{
		bool CanApply(IGameState gameState);
		void Apply(IGameState gameState);
	}
}

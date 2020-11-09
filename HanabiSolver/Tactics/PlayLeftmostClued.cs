using HanabiSolver.Library.Game;
using System.Linq;

namespace HanabiSolver.Tactics
{
	public class PlayLeftmostClued : ITactics
	{
		private readonly InformationMemento informationMemento = new InformationMemento();
		
		public bool CanApply(IGameState gameState)
		{
			var hasNewInformation = informationMemento.Update(gameState.CurrentPlayer);
			var hasUnknownCardsToTheRight = false;

			if (hasNewInformation)
			{
				var leftmostCluedCard = LeftmostCluedCard(gameState.CurrentPlayer);

				hasUnknownCardsToTheRight = gameState.CurrentPlayer.Cards
					.SkipWhile(c => c != leftmostCluedCard)
					.Skip(1)
					.Any(c => Utils.IsUnknownCard(gameState.CurrentPlayer, c));
			}

			return hasNewInformation && hasUnknownCardsToTheRight;
		}

		public void Apply(IGameState gameState)
		{
			var leftmostCluedCard = LeftmostCluedCard(gameState.CurrentPlayer);

			gameState.CurrentPlayer.Play(leftmostCluedCard);
		}

		private Card LeftmostCluedCard(IPlayer player)
		{
			return informationMemento
				.GetNewInformation(player)
				.First();
		}
	}
}

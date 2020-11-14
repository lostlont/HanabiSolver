using HanabiSolver.Library.Game;
using System.Collections.Generic;
using System.Linq;

namespace HanabiSolver
{
	public class InformationMemento
	{
		private readonly Dictionary<IPlayer, IReadOnlyDictionary<Card, IReadOnlyInformation>> lastPlayerInformation = new Dictionary<IPlayer, IReadOnlyDictionary<Card, IReadOnlyInformation>>();
		private readonly Dictionary<IPlayer, IReadOnlyList<Card>> newPlayerInformation = new Dictionary<IPlayer, IReadOnlyList<Card>>();

		public bool Update(IPlayer player)
		{
			lastPlayerInformation.TryGetValue(player, out var lastInformation);

			var newInformation = (lastInformation != null)
				? ListNewInformation(lastInformation, player)
				: player.Cards.Where(c => !Utils.IsUnknownCard(player, c)).ToList();

			lastPlayerInformation[player] = Copy(player.Information);
			newPlayerInformation[player] = newInformation;
			return newInformation.Any();
		}

		public IReadOnlyList<Card> GetNewInformation(IPlayer player)
		{
			return newPlayerInformation[player];
		}

		private List<Card> ListNewInformation(IReadOnlyDictionary<Card, IReadOnlyInformation> lastInformation, IPlayer currentPlayer)
		{
			var result = new List<Card>();

			foreach (var currentCard in currentPlayer.Cards)
			{
				var currentInformationOnCard = currentPlayer.Information[currentCard];

				if (lastInformation.TryGetValue(currentCard, out var lastInformationOnCard))
				{
					if (!Equals(lastInformationOnCard, currentInformationOnCard))
						result.Add(currentCard);
				}
				else if (!Utils.IsUnknown(currentInformationOnCard))
				{
					result.Add(currentCard);
				}
			}

			return result;
		}

		private bool Equals(IReadOnlyInformation left, IReadOnlyInformation right)
		{
			return
				(left.IsSuiteKnown == right.IsSuiteKnown) &&
				(left.IsNumberKnown == right.IsNumberKnown);
		}

		private IReadOnlyDictionary<Card, IReadOnlyInformation> Copy(IReadOnlyDictionary<Card, IReadOnlyInformation> information)
		{
			return information.ToDictionary(
				e => e.Key,
				e => (IReadOnlyInformation)new Information
				{
					IsSuiteKnown = e.Value.IsSuiteKnown,
					IsNumberKnown = e.Value.IsNumberKnown,
				});
		}
	}
}

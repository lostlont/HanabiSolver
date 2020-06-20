using System.Linq;

namespace HanabiSolver.Library.Game
{
	public partial class Deck
	{
		public static Deck Empty => new Deck(Enumerable.Empty<Card>());
	}
}

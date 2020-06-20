namespace HanabiSolver.Library.Game
{
	public class Card
	{
		private readonly Suite suite;
		private readonly Number value;

		public Card(Suite suite, Number value)
		{
			this.suite = suite;
			this.value = value;
		}
	}
}

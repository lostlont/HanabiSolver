namespace HanabiSolver.Library.Game
{
	public class Card
	{
		private readonly Suite suite;
		private readonly Number number;

		public Card(Suite suite, Number number)
		{
			this.suite = suite;
			this.number = number;
		}

		public override string ToString() => $"Card({suite}, {number})";
	}
}

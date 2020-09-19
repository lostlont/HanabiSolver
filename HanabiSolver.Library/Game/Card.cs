namespace HanabiSolver.Library.Game
{
	public class Card
	{
		private readonly Number number;

		public Suite Suite { get; }

		public Card(Suite suite, Number number)
		{
			Suite = suite;
			this.number = number;
		}

		public override string ToString() => $"Card({Suite}, {number})";
	}
}

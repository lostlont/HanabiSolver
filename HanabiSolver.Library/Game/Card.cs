namespace HanabiSolver.Library.Game
{
	public class Card
	{
		public Suite Suite { get; }
		public Number Number { get; }

		public Card(Suite suite, Number number)
		{
			Suite = suite;
			Number = number;
		}

		public override string ToString() => $"Card({Suite}, {Number})";
	}
}

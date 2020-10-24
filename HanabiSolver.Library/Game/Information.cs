namespace HanabiSolver.Library.Game
{
	public interface IReadOnlyInformation
	{
		bool IsSuiteKnown { get; }
		bool IsNumberKnown { get; }
	}

	public class Information : IReadOnlyInformation
	{
		public bool IsSuiteKnown { get; set; }
		public bool IsNumberKnown { get; set; }
	}
}

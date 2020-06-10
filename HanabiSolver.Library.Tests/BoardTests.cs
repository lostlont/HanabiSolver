using Xunit;

namespace HanabiSolver.Library.Tests
{
	public class BoardTests
	{
		[Fact]
		public void BoardCanBeCreated()
		{
			var board = new Board();
			Assert.NotNull(board);
		}
	}
}

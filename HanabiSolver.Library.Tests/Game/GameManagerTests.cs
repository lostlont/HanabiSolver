using FluentAssertions;
using HanabiSolver.Library.Game;
using HanabiSolver.Library.Tests.Builders;
using Moq;
using Xunit;

namespace HanabiSolver.Library.Tests.Game
{
	public class GameManagerTests
	{
		[Fact]
		public void SimulateReturnsImmediatelyForFinishedState()
		{
			var fuseTokens = new Mock<ITokens>(MockBehavior.Strict);
			fuseTokens
				.Setup(t => t.MaxAmount)
				.Returns(3);
			fuseTokens
				.Setup(t => t.Amount)
				.Returns(3);
			var gameState = new GameStateBuilder
			{
				Table = new TableBuilder
				{
					FuseTokens = fuseTokens.Object,
				}.Build(),
			}.Build();
			var gameManager = new GameManager(gameState);

			gameManager.Simulate();

			gameState.IsEnded.Should().BeTrue();
		}

		// TODO Tests for simulating with non-finished states.
	}
}

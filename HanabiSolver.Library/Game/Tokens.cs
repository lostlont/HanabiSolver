using System;

namespace HanabiSolver.Library.Game
{
	public interface IReadOnlyTokens
	{
		int MaxAmount { get; }
		int Amount { get; }
	}

	public interface ITokens : IReadOnlyTokens
	{
		void Use();
		void Replenish();
	}

	public class Tokens : ITokens
	{
		public int MaxAmount { get; }

		public int Amount { get; private set; }

		public Tokens(int maxAmount)
			: this(maxAmount, maxAmount)
		{
			MaxAmount = maxAmount;
			Amount = maxAmount;
		}

		public Tokens(int maxAmount, int amount)
		{
			MaxAmount = (0 < maxAmount)
				? maxAmount
				: throw new ArgumentOutOfRangeException(nameof(maxAmount));

			Amount = ((0 <= amount) && (amount <= maxAmount))
				? amount
				: throw new ArgumentOutOfRangeException(nameof(amount));
		}

		public void Use()
		{
			Amount = Math.Max(Amount - 1, 0);
		}

		public void Replenish()
		{
			Amount = Math.Min(Amount + 1, MaxAmount);
		}
	}
}

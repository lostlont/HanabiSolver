using System;

namespace HanabiSolver.Library.Game
{
	public class Tokens
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

		public void Remove()
		{
			Amount = Math.Max(Amount - 1, 0);
		}

		public void Add()
		{
			Amount = Math.Min(Amount + 1, MaxAmount);
		}
	}
}

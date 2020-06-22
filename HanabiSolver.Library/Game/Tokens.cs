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
			if (Amount <= 0)
				throw new InvalidOperationException();

			Amount -= 1;
		}

		public void Add()
		{
			if (Amount >= MaxAmount)
				throw new InvalidOperationException();

			Amount += 1;
		}
	}
}

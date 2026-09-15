using System;
using System.Threading.Tasks;

namespace yuuki.Scripts;

public static class YukiCrystalSystem
{
	private static int _snowCrystals;
	private static int _maxSnowCrystals = 9;

	public static int MaxSnowCrystals
	{
		get => _maxSnowCrystals;
		set
		{
			_maxSnowCrystals = value;
			OnMaxCrystalsChanged?.Invoke(_maxSnowCrystals);
			CurrentCrystals = _snowCrystals;
		}
	}

	public static int CurrentCrystals
	{
		get => _snowCrystals;
		set
		{
			_snowCrystals = Math.Clamp(value, 0, MaxSnowCrystals);
			OnCrystalsChanged?.Invoke(_snowCrystals);
		}
	}

	public static int ConsumedCrystalsThisCombat { get; private set; }

	public static event Action<int>? OnCrystalsChanged;
	public static event Action<int>? OnMaxCrystalsChanged;
	public static event Func<int, Task>? OnCrystalGained;

	public static void Reset()
	{
		_snowCrystals = 0;
		_maxSnowCrystals = 9;
		ConsumedCrystalsThisCombat = 0;
		OnCrystalsChanged = null;
		OnMaxCrystalsChanged = null;
		OnCrystalGained = null;
	}

	public static async Task AddCrystals(int amount = 1)
	{
		if (amount < 0)
		{
			ConsumedCrystalsThisCombat += Math.Abs(amount);
		}

		CurrentCrystals += amount;
		if (amount <= 0 || OnCrystalGained is null)
		{
			return;
		}

		foreach (Func<int, Task> handler in OnCrystalGained.GetInvocationList())
		{
			await handler(amount);
		}
	}
}

using System;

namespace yuuki.Scripts;

public static class YukiCrystalSystem
{
	private static int _snowCrystals = 0;

	private static int _maxSnowCrystals = 9;

	public static int MaxSnowCrystals
	{
		get
		{
			return _maxSnowCrystals;
		}
		set
		{
			_maxSnowCrystals = value;
			YukiCrystalSystem.OnMaxCrystalsChanged?.Invoke(_maxSnowCrystals);
			CurrentCrystals = _snowCrystals;
		}
	}

	public static int CurrentCrystals
	{
		get
		{
			return _snowCrystals;
		}
		set
		{
			_snowCrystals = Math.Max(0, Math.Min(value, MaxSnowCrystals));
			YukiCrystalSystem.OnCrystalsChanged?.Invoke(_snowCrystals);
		}
	}

	public static int ConsumedCrystalsThisCombat { get; private set; }

	public static event Action<int>? OnCrystalsChanged;

	public static event Action<int>? OnMaxCrystalsChanged;

	public static event Action<int>? OnCrystalGained;

	public static void Reset()
	{
		_snowCrystals = 0;
		MaxSnowCrystals = 9;
		ConsumedCrystalsThisCombat = 0;
		YukiCrystalSystem.OnCrystalsChanged = null;
		YukiCrystalSystem.OnMaxCrystalsChanged = null;
		YukiCrystalSystem.OnCrystalGained = null;
		YukiCrystalSystem.OnCrystalsChanged?.Invoke(0);
	}

	public static void AddCrystals(int amount = 1)
	{
		if (amount < 0)
		{
			ConsumedCrystalsThisCombat += Math.Abs(amount);
		}
		CurrentCrystals += amount;
		if (amount > 0)
		{
			YukiCrystalSystem.OnCrystalGained?.Invoke(amount);
		}
	}
}

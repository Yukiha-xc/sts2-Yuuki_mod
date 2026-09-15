using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts;

public class YukiCrystalVar : DynamicVar
{
	public const string Key = "YukiCrystal";

	public static readonly string LocKey = "YUUKI_SNOW_CRYSTAL";

	public YukiCrystalVar(decimal baseValue)
		: base("YukiCrystal", baseValue)
	{
		DynamicVarExtensions.WithTooltip<YukiCrystalVar>(this, LocKey, "static_hover_tips");
	}
}

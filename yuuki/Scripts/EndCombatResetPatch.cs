using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;

namespace yuuki.Scripts;

[HarmonyPatch(typeof(CombatManager), "EndCombatInternal")]
public static class EndCombatResetPatch
{
	public static void Postfix()
	{
		YukiCrystalSystem.Reset();
	}
}

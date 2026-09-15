using System;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;

namespace yuuki.Scripts;

[HarmonyPatch]
public static class EndCombatResetPatch
{
	public static MethodBase TargetMethod()
	{
		return AccessTools.DeclaredMethod(typeof(CombatManager), "EndCombatInternal", Type.EmptyTypes)
			?? throw new MissingMethodException(typeof(CombatManager).FullName, "EndCombatInternal()");
	}

	public static void Postfix(ref Task __result)
	{
		__result = ResetAfterCombatEnds(__result);
	}

	private static async Task ResetAfterCombatEnds(Task endCombatTask)
	{
		try
		{
			await endCombatTask;
		}
		finally
		{
			YukiCrystalSystem.Reset();
		}
	}
}

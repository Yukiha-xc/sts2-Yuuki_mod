using System;
using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace yuuki.Scripts.Patches;

public static class AnimationPatches
{
	[HarmonyPatch]
	public static class MerchantFix
	{
		[HarmonyTargetMethod]
		public static MethodBase Target()
		{
			return AccessTools.Method("MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantCharacter:PlayAnimation", (Type[])null, (Type[])null);
		}

		[HarmonyPrefix]
		public static bool Prefix(Node __instance)
		{
			return true;
		}
	}
}

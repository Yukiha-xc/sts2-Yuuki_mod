using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;
using System;

namespace yuuki.Scripts.Patches;

[HarmonyPatch(typeof(NCreature), "SetAnimationTrigger")]
public static class AnimationPatches
{
    public static void Postfix(NCreature __instance, string trigger)
    {
        
        if (__instance.Entity != null && __instance.Entity.ModelId.Entry.Contains("YUUKI"))
        {
            var visuals = __instance.Visuals;
            if (visuals == null) return;

            
            if (trigger.IndexOf("attack", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                
                
                var sprite = visuals.GetNodeOrNull<Sprite2D>("Visuals");
                
                if (sprite != null)
                {
                    GD.Print("[AnimationMonitor] FOUND Sprite2D! Executing direct tween.");
                    
                    
                    var tween = visuals.CreateTween();
                    
                    tween.TweenProperty(sprite, "position:x", 80, 0.1);
                    tween.TweenProperty(sprite, "position:x", 0, 0.1);
                }
                else
                {
                    GD.Print("[AnimationMonitor] ERROR: Could not find 'Visuals' node under YukiCharacter!");
                }
            }
        }
    }

    [HarmonyPatch]
    public static class MerchantFix
    {
        [HarmonyTargetMethod]
        public static System.Reflection.MethodBase Target() => AccessTools.Method("MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantCharacter:PlayAnimation");
        [HarmonyPrefix]
        public static bool Prefix(Node __instance) {
            return true;
        }
    }
}

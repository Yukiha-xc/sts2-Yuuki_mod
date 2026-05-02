using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Godot.Bridge;

namespace yuuki.Scripts;

[ModInitializer("Init")]
public class Entry
{
    public static void Init()
    {
        try
        {
            
            var harmony = new Harmony("sts2.yuukimod");
            harmony.PatchAll();
            Log.Info("YukiMod: Harmony patches applied successfully.");

            
            ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
            
            Log.Info("YukiMod initialized successfully!");
        }
        catch (Exception e)
        {
            Log.Error($"YukiMod initialization failed: {e}");
        }
    }
}

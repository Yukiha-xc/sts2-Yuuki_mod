using System;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace yuuki.Scripts;

[ModInitializer("Init")]
public class Entry
{
	public static void Init()
	{
		try
		{
			new Harmony("sts2.yuukimod").PatchAll();
			Log.Info("YukiMod: Harmony patches applied successfully.");
			
			BaseLib.Config.ModConfigRegistry.Register("yuuki", new YukiModConfig());
			
			ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
			Log.Info("YukiMod initialized successfully!");
		}
		catch (Exception value)
		{
			Log.Error($"YukiMod initialization failed: {value}");
		}
	}
}

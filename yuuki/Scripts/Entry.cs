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
		}
		catch (Exception value)
		{
			Log.Error($"YukiMod Harmony initialization failed: {value}");
		}

		try
		{
			BaseLib.Config.ModConfigRegistry.Register("yuuki", new YukiModConfig());
			Log.Info("YukiMod config registered successfully.");
		}
		catch (Exception value)
		{
			Log.Error($"YukiMod config initialization failed: {value}");
		}

		try
		{
			ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
			Log.Info("YukiMod initialized successfully!");
		}
		catch (Exception value)
		{
			Log.Error($"YukiMod script registration failed: {value}");
		}
	}
}

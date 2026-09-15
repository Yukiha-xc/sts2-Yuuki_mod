using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts;

[HarmonyPatch(typeof(NEnergyCounter), "_Process")]
public static class EnergyCounterDisplayPatch
{
	private sealed class CounterState
	{
		public CounterState(TextureRect crystalBackground, Label crystalLabel)
		{
			CrystalBackground = crystalBackground;
			CrystalLabel = crystalLabel;
		}

		public TextureRect CrystalBackground { get; }
		public Label CrystalLabel { get; }
		public int LastCrystals { get; set; } = -1;
		public float PulseTimer { get; set; }
		public float JuiceScale { get; set; }
	}

	private static readonly ConditionalWeakTable<NEnergyCounter, CounterState> States = new();

	public static void Postfix(NEnergyCounter __instance, double delta)
	{
		if (!States.TryGetValue(__instance, out CounterState? state))
		{
			Label? crystalLabel = __instance.GetNodeOrNull<Label>("%SnowCrystalLabel");
			TextureRect? crystalBackground = __instance.GetNodeOrNull<TextureRect>("%SnowCrystalBg");
			if (crystalLabel is null || crystalBackground is null)
			{
				return;
			}

			__instance.GetNodeOrNull<Label>("Label")?.AddThemeColorOverride("font_color", Colors.Black);
			ConnectSignals(__instance);
			state = new CounterState(crystalBackground, crystalLabel);
			States.Add(__instance, state);
		}

		state.PulseTimer += (float)delta;
		float pulseScale = 1f + 0.015f * Mathf.Sin(state.PulseTimer * Mathf.Pi * 0.5f);
		__instance.Scale = new Vector2(pulseScale, pulseScale);
		__instance.PivotOffset = new Vector2(60f, 60f);

		int crystals = YukiCrystalSystem.CurrentCrystals;
		state.CrystalLabel.Text = crystals.ToString();
		if (state.LastCrystals >= 0 && state.LastCrystals != crystals)
		{
			state.JuiceScale = 0.4f;
		}
		state.LastCrystals = crystals;

		if (state.JuiceScale > 0.001f)
		{
			state.JuiceScale = Mathf.Lerp(state.JuiceScale, 0f, (float)delta * 8f);
			float crystalScale = 1f + state.JuiceScale;
			state.CrystalBackground.Scale = new Vector2(crystalScale, crystalScale);
			state.CrystalBackground.PivotOffset = state.CrystalBackground.Size / 2f;
		}
		else
		{
			state.CrystalBackground.Scale = Vector2.One;
		}
	}

	private static void ConnectSignals(Control root)
	{
		root.MouseEntered += delegate
		{
			ShowTip(root, "YUUKI_ENERGY", includeEmpathy: false);
		};
		root.MouseExited += delegate
		{
			HideTip(root);
		};
		Control? crystalBg = root.GetNodeOrNull<Control>("%SnowCrystalBg");
		if (crystalBg is not null)
		{
			crystalBg.MouseEntered += delegate
			{
				ShowTip(crystalBg, "YUUKI_SNOW_CRYSTAL", includeEmpathy: true);
			};
			crystalBg.MouseExited += delegate
			{
				HideTip(crystalBg);
			};
		}
	}

	private static void ShowTip(Control target, string key, bool includeEmpathy)
	{
		try
		{
			// A child control can become hovered while the counter itself is still
			// hovered. Clear any existing set before registering the new target.
			NHoverTipSet.Remove(target);
			LocString title = new("static_hover_tips", key + ".title");
			LocString description = new("static_hover_tips", key + ".description");
			List<IHoverTip> tips = [new HoverTip(title, description)];
			if (includeEmpathy)
			{
				tips.Add(HoverTipFactory.FromPower<EmpathyPower>());
			}
			NHoverTipSet.CreateAndShow(target, tips);
		}
		catch (System.Exception ex)
		{
			Log.Error("Failed to show Yuki hover tip: " + ex.Message);
		}
	}

	private static void HideTip(Control target)
	{
		NHoverTipSet.Remove(target);
	}
}

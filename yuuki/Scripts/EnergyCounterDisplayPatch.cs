using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
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
	private static int _lastCrystals = -1;

	private static float _pulseTimer = 0f;

	private static float _juiceScale = 0f;

	public static void Postfix(Node __instance, double delta)
	{
		try
		{
			Control val = (Control)(object)((__instance is Control) ? __instance : null);
			if (val == null)
			{
				return;
			}
			if (!((GodotObject)val).HasMeta("yuuki_signals_connected"))
			{
				ConnectSignals(val);
				((GodotObject)val).SetMeta("yuuki_signals_connected", Variant.From(true));
			}
			_pulseTimer += (float)delta;
			Label nodeOrNull = ((Node)val).GetNodeOrNull<Label>("Label");
			if (nodeOrNull != null)
			{
				CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
				bool flag = false;
				if (combatState != null)
				{
					flag = combatState.Players.FirstOrDefault((Player p) => LocalContext.IsMe(p))?.Character is YukiCharacter;
				}
				if (flag)
				{
					((Control)nodeOrNull).AddThemeColorOverride("font_color", Colors.Black);
				}
				else
				{
					((Control)nodeOrNull).RemoveThemeColorOverride("font_color");
				}
			}
			float num = 1f + 0.015f * Mathf.Sin(_pulseTimer * (float)Math.PI * 0.5f);
			val.Scale = new Vector2(num, num);
			val.PivotOffset = new Vector2(60f, 60f);
			TextureRect nodeOrNull2 = ((Node)val).GetNodeOrNull<TextureRect>("%SnowCrystalBg");
			Label nodeOrNull3 = ((Node)val).GetNodeOrNull<Label>("%SnowCrystalLabel");
			if (nodeOrNull3 != null)
			{
				nodeOrNull3.Text = YukiCrystalSystem.CurrentCrystals.ToString();
			}
			if (nodeOrNull2 != null)
			{
				if (_lastCrystals != -1 && _lastCrystals != YukiCrystalSystem.CurrentCrystals)
				{
					_juiceScale = 0.4f;
				}
				_lastCrystals = YukiCrystalSystem.CurrentCrystals;
				if (_juiceScale > 0.001f)
				{
					_juiceScale = Mathf.Lerp(_juiceScale, 0f, (float)delta * 8f);
					float num2 = 1f + _juiceScale;
					((Control)nodeOrNull2).Scale = new Vector2(num2, num2);
					((Control)nodeOrNull2).PivotOffset = ((Control)nodeOrNull2).Size / 2f;
				}
				else
				{
					((Control)nodeOrNull2).Scale = Vector2.One;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void ConnectSignals(Control root)
	{
		root.MouseEntered += delegate
		{
			ShowTip(root, "YUUKI_ENERGY", includeEmpathy: false);
		};
		root.MouseExited += HideTip;
		Control crystalBg = ((Node)root).GetNodeOrNull<Control>("%SnowCrystalBg");
		if (crystalBg != null)
		{
			crystalBg.MouseEntered += delegate
			{
				ShowTip(crystalBg, "YUUKI_SNOW_CRYSTAL", includeEmpathy: true);
			};
			crystalBg.MouseExited += HideTip;
		}
	}

	private static void ShowTip(Control target, string key, bool includeEmpathy)
	{
		try
		{
			LocString title = new LocString("static_hover_tips", key + ".title");
			LocString description = new LocString("static_hover_tips", key + ".description");
			HoverTip hoverTip = new HoverTip(title, description);
			List<IHoverTip> list = new List<IHoverTip> { hoverTip };
			if (includeEmpathy)
			{
				list.Add(HoverTipFactory.FromPower<EmpathyPower>());
			}
			NHoverTipSet.CreateAndShow(target, list);
		}
		catch (Exception ex)
		{
			Log.Error("Failed to show hover tip: " + ex.Message);
		}
	}

	private static void HideTip()
	{
		NHoverTipSet.Clear();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Creatures;
using BaseLib.Extensions;

namespace yuuki.Scripts;

public static class YukiCrystalSystem
{
    private static int _snowCrystals = 0;
    private static int _maxSnowCrystals = 9;
    public static int MaxSnowCrystals
    {
        get => _maxSnowCrystals;
        set
        {
            _maxSnowCrystals = value;
            OnMaxCrystalsChanged?.Invoke(_maxSnowCrystals);
            CurrentCrystals = _snowCrystals;
        }
    }

    public static int CurrentCrystals
    {
        get => _snowCrystals;
        set
        {
            _snowCrystals = Math.Max(0, Math.Min(value, MaxSnowCrystals));
            OnCrystalsChanged?.Invoke(_snowCrystals);
        }
    }

    public static event Action<int>? OnCrystalsChanged;
    public static event Action<int>? OnMaxCrystalsChanged;
    public static event Action<int>? OnCrystalGained; 

    public static int ConsumedCrystalsThisCombat { get; private set; }

    public static void Reset()
    {
        _snowCrystals = 0;
        MaxSnowCrystals = 9;
        ConsumedCrystalsThisCombat = 0;
        
        OnCrystalsChanged = null;
        OnMaxCrystalsChanged = null;
        OnCrystalGained = null;

        OnCrystalsChanged?.Invoke(0);
    }

    public static void AddCrystals(int amount = 1)
    {
        if (amount < 0)
        {
            ConsumedCrystalsThisCombat += Math.Abs(amount);
        }
        
        CurrentCrystals += amount;
        if (amount > 0)
        {
            OnCrystalGained?.Invoke(amount); 
        }
    }
}


public class YukiCrystalVar : DynamicVar
{
    public const string Key = "YukiCrystal";
    public static readonly string LocKey = "YUUKI_SNOW_CRYSTAL"; 

    public YukiCrystalVar(decimal baseValue) : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}

[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Nodes.Combat.NEnergyCounter), "_Process")]
public static class EnergyCounterDisplayPatch
{
    private static int _lastCrystals = -1;
    private static float _pulseTimer = 0f;
    private static float _juiceScale = 0f;

    public static void Postfix(Node __instance, double delta)
    {
        try
        {
            if (__instance is not Control control) return;

            if (!control.HasMeta("yuuki_signals_connected"))
            {
                ConnectSignals(control);
                control.SetMeta("yuuki_signals_connected", true);
            }

            _pulseTimer += (float)delta;
            var energyLabel = control.GetNodeOrNull<Label>("Label");
            if (energyLabel != null)
            {
                var combatState = MegaCrit.Sts2.Core.Combat.CombatManager.Instance.DebugOnlyGetState();
                var isYuki = false;
                if (combatState != null)
                {
                    var me = combatState.Players.FirstOrDefault(p => MegaCrit.Sts2.Core.Context.LocalContext.IsMe(p));
                    isYuki = me?.Character is YukiCharacter;
                }

                if (isYuki)
                {
                    energyLabel.AddThemeColorOverride("font_color", Colors.Black);
                }
                else
                {
                    energyLabel.RemoveThemeColorOverride("font_color");
                }
            }

            float pulse = 1.0f + 0.015f * Mathf.Sin(_pulseTimer * Mathf.Pi * 0.5f);
            control.Scale = new Vector2(pulse, pulse);
            control.PivotOffset = new Vector2(60, 60);

            var crystalBg = control.GetNodeOrNull<TextureRect>("%SnowCrystalBg");
            var crystalLabel = control.GetNodeOrNull<Label>("%SnowCrystalLabel");

            if (crystalLabel != null)
            {
                crystalLabel.Text = YukiCrystalSystem.CurrentCrystals.ToString();
            }

            if (crystalBg != null)
            {
                if (_lastCrystals != -1 && _lastCrystals != YukiCrystalSystem.CurrentCrystals)
                {
                    _juiceScale = 0.4f;
                }
                _lastCrystals = YukiCrystalSystem.CurrentCrystals;

                if (_juiceScale > 0.001f)
                {
                    _juiceScale = Mathf.Lerp(_juiceScale, 0, (float)delta * 8.0f);
                    float finalScale = 1.0f + _juiceScale;
                    crystalBg.Scale = new Vector2(finalScale, finalScale);
                    crystalBg.PivotOffset = crystalBg.Size / 2;
                }
                else
                {
                    crystalBg.Scale = Vector2.One;
                }
            }
        }
        catch (Exception) {}
    }


    private static void ConnectSignals(Control root)
    {
        root.MouseEntered += () => ShowTip(root, "YUUKI_ENERGY", false);
        root.MouseExited += HideTip;

        var crystalBg = root.GetNodeOrNull<Control>("%SnowCrystalBg");
        if (crystalBg != null)
        {
            crystalBg.MouseEntered += () => ShowTip(crystalBg, "YUUKI_SNOW_CRYSTAL", true);
            crystalBg.MouseExited += HideTip;
        }
    }

    private static void ShowTip(Control target, string key, bool includeEmpathy)
    {
        try
        {
            var title = new LocString("static_hover_tips", $"{key}.title");
            var desc = new LocString("static_hover_tips", $"{key}.description");
            var tip = new HoverTip(title, desc, null);
            
            var tipsList = new List<IHoverTip> { tip };
            if (includeEmpathy)
            {
                tipsList.Add(HoverTipFactory.FromPower<yuuki.Scripts.Powers.EmpathyPower>());
            }
            
            NHoverTipSet.CreateAndShow(target, tipsList, HoverTipAlignment.None);
        }
        catch (Exception e)
        {
            Log.Error("Failed to show hover tip: " + e.Message);
        }
    }

    private static void HideTip()
    {
        NHoverTipSet.Clear();
    }
}

[HarmonyPatch(typeof(MegaCrit.Sts2.Core.Combat.CombatManager), "EndCombatInternal")]
public static class EndCombatResetPatch
{
    public static void Postfix()
    {
        YukiCrystalSystem.Reset();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class EternalGift : CustomRelicModel
{
    protected override string BigIconPath => "res://yuuki/images/relics/EternalGift.png";
    public override string PackedIconPath => "res://yuuki/images/relics/EternalGift.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/EternalGift.png";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (dealer != null && dealer.IsPlayer)
        {
            int crystals = YukiCrystalSystem.CurrentCrystals;
            
            if (crystals < 3) return 0.90m;
            if (crystals == 3) return 1.0m;
            return 1.0m + (crystals - 3) * 0.10m;
        }
        return 1.0m;
    }

    
    
    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        int crystals = YukiCrystalSystem.CurrentCrystals;

        
        if (crystals < 3)
        {
            await CardPileCmd.Draw(choiceContext, 1, player);
            YukiCrystalSystem.AddCrystals(1);
        }
        
        else if (crystals >= 8)
        {
            YukiCrystalSystem.AddCrystals(-2);
            var combatState = MegaCrit.Sts2.Core.Combat.CombatManager.Instance.DebugOnlyGetState();
            if (combatState != null)
            {
                var aliveEnemies = combatState.Enemies.Where(e => e.IsAlive).ToList();
                if (aliveEnemies.Count > 0)
                {
                    var targets = aliveEnemies.Where(e => !e.HasPower<yuuki.Scripts.Powers.EmpathyPower>()).ToList();
                    if (targets.Count == 0) targets = aliveEnemies;
                    
                    var randomEnemy = targets[new System.Random().Next(targets.Count)];
                    await PowerCmd.Apply<yuuki.Scripts.Powers.EmpathyPower>(new ThrowingPlayerChoiceContext(), randomEnemy, 1m, player.Creature, null);
                }
            }
        }
    }

    public override async Task BeforeCombatStart()
    {
        
        YukiCrystalSystem.Reset();
        YukiCrystalSystem.AddCrystals(6);
        await Task.CompletedTask;
    }
}


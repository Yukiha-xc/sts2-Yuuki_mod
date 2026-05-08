using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Entities.Relics;
using yuuki.Scripts;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class SecretTreasure : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    
    public override string PackedIconPath => "res://yuuki/images/relics/relics1.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/relics1.png";
    protected override string BigIconPath => "res://yuuki/images/relics/relics1.png";

    
    public override async Task BeforeCombatStart()
    {
        
        var combatState = base.Owner.Creature.CombatState;
        if (combatState == null) return;

        
        var enemies = combatState.HittableEnemies.ToList();
        if (enemies.Count > 0)
        {
            
            var target = base.Owner.RunState.Rng.CombatTargets.NextItem(enemies);
            if (target != null)
            {
                
                this.Flash();
                
                await PowerCmd.Apply<EmpathyPower>(new ThrowingPlayerChoiceContext(), target, 1m, base.Owner.Creature, null);
            }
        }
    }
}


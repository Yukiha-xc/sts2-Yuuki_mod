using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using BaseLib.Abstracts;
using BaseLib.Utils;

namespace yuuki.Scripts;

[Pool(typeof(MegaCrit.Sts2.Core.Models.RelicPools.SharedRelicPool))]
public sealed class BitterChocolateRelic : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => "res://yuuki/images/relics/bitter_chocolate_relic.png";
    protected override string PackedIconOutlinePath => "res://yuuki/images/relics/bitter_chocolate_relic.png";
    protected override string BigIconPath => "res://yuuki/images/relics/bitter_chocolate_relic.png";

    public override async Task AfterObtained()
    {
        await CreatureCmd.GainMaxHp(base.Owner.Creature, 10m);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        var combatState = base.Owner.Creature.CombatState;
        
        if (player == base.Owner && combatState != null && combatState.RoundNumber <= 1 && player.RunState.CurrentActIndex == 1)
        {
            Flash();
            await CreatureCmd.Heal(base.Owner.Creature, 6);
            
            if (combatState.HittableEnemies.Count > 0)
            {
                await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, 8m, ValueProp.Unpowered, base.Owner.Creature);
            }
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;
using yuuki.Scripts.Cards;

namespace yuuki.Scripts.Powers;

public sealed class DiaryPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7m, ValueProp.Move)
    ];

    public void SetDamage(decimal damage)
    {
        AssertMutable();
        base.DynamicVars.Damage.BaseValue = damage;
    }

    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != base.Owner.Player)
        {
            return;
        }

        if (base.Amount > 1)
        {
            await PowerCmd.Decrement(this);
            return;
        }

        Flash();
        
        SfxCmd.Play("heavy_attack.mp3");
        
        await CreatureCmd.Damage(
            choiceContext, 
            base.CombatState.HittableEnemies, 
            base.DynamicVars.Damage, 
            base.Owner
        );

        await PowerCmd.Remove(this);
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";
}

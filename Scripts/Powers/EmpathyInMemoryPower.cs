using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class EmpathyInMemoryPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        ..base.ExtraHoverTips,
        HoverTipFactory.FromPower<EmpathyPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move)
    ];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (cardSource != null && cardSource.IsUpgraded)
        {
            base.DynamicVars.Damage.UpgradeValueBy(3m);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, MegaCrit.Sts2.Core.Entities.Players.Player player)
    {
        if (player == base.Owner.Player)
        {
            
            var target = base.CombatState.Enemies.FirstOrDefault(e => e.IsAlive && e.HasPower<EmpathyPower>());
            if (target != null)
            {
                Flash();
                await PowerCmd.Remove<EmpathyPower>(target);
            }
        }
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        
        
        if (power is EmpathyPower && amount != 0m)
        {
            var target = power.Owner;
            if (target != null && target.IsAlive)
            {
                Flash();
                decimal damageValue = base.DynamicVars.Damage.BaseValue;
                
                var choiceContext = new ThrowingPlayerChoiceContext();
                await CreatureCmd.Damage(choiceContext, target, damageValue, ValueProp.Move, base.Owner, null);
            }
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";
}

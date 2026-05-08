using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;

using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public class EmpathyPower : CustomPowerModel
{
    
    public override PowerType Type => PowerType.Debuff;

    
    public override PowerStackType StackType => PowerStackType.None;

    
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Owner)
        {
            int empathyCount = base.CombatState.Enemies.Count(e => e.IsAlive && e.HasPower<EmpathyPower>());
            return 1.0m + 0.25m * empathyCount;
        }
        return 1m;
    }

    
    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (amount > 0m && creature == base.Owner)
        {
            var player = base.CombatState.PlayerCreatures.FirstOrDefault();
            if (player != null)
            {
                decimal multiplier = 0.25m;
                decimal blockToGain = amount * multiplier;
                if (blockToGain > 0)
                {
                    this.Flash();
                    await PowerCmd.Apply<BlockNextTurnPower>(new ThrowingPlayerChoiceContext(), player, blockToGain, player, null);
                }
            }
        }
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        
        await TriggerEmpathyInMemoryDamage(base.Owner);
    }

    public override async Task AfterRemoved(Creature owner)
    {
        await base.AfterRemoved(owner);
        
        YukiCrystalSystem.AddCrystals(1);
        
        await TriggerEmpathyInMemoryDamage(owner);
    }

    private async Task TriggerEmpathyInMemoryDamage(Creature target)
    {
        
        if (target == null || !target.IsAlive)
        {
            return;
        }

        var player = base.CombatState.PlayerCreatures.FirstOrDefault();
        if (player != null && player.HasPower<EmpathyInMemoryPower>())
        {
            var memoryPower = player.GetPower<EmpathyInMemoryPower>();
            
            decimal damageValue = memoryPower.DynamicVars.Damage.BaseValue * memoryPower.Amount;
            
            
            if (target.IsAlive)
            {
                this.Flash();
                var choiceContext = new ThrowingPlayerChoiceContext();
                await CreatureCmd.Damage(choiceContext, target, damageValue, ValueProp.Move, player, null);
            }
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyPower.png";
}


using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Combat;

using
BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class PrayerPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
public override PowerStackType StackType => PowerStackType.Counter;
public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        
if (target !=
base.Owner || dealer == null)
        {
return 1m;
        }

        
if (!dealer.HasPower<EmpathyPower>())
        {
return 1m;
        }

        
return 0.5m;
    }

    
public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
if (side == CombatSide.Enemy)
        {
await PowerCmd.TickDownDuration(this);
        }
    }
public override string CustomPackedIconPath => "res://yuuki/images/powers/PrayerPower.png";
public override string CustomBigIconPath => "res://yuuki/images/powers/PrayerPower.png";
}



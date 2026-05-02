using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Cards;

using
BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;
public sealed class NightSkyOfPowerUserPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
    
    
    public override PowerStackType StackType => PowerStackType.Counter;
public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        
        if (dealer != base.Owner)
        {
return 1m;
        }

        
        {
return 1m;
        }

        
        {
            
            return 1m + (decimal)base.Amount / 100m;
        }
return 1m;
    }
public override string CustomPackedIconPath => "res://yuuki/images/powers/NightSkyOfPowerUserPower.png";
public override string CustomBigIconPath => "res://yuuki/images/powers/NightSkyOfPowerUserPower.png";
}



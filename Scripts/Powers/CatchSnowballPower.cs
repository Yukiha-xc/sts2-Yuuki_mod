using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace yuuki.Scripts.Powers;
public class CatchSnowballPower : CustomPowerModel
{
public override PowerType Type => PowerType.Buff;
    
public override PowerStackType StackType => PowerStackType.Counter;
    
    
protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(0m, ValueProp.Unpowered)
    ];
protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    
public void SetBlock(decimal block)
    {
        AssertMutable();
base.DynamicVars.Block.BaseValue = block;
    }
public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        
if (player ==
base.Owner.Player &&
base.AmountOnTurnStart > 0)
        {
            
if (YukiCrystalSystem.CurrentCrystals >= 3)
            {
this.Flash();
                
                
await CreatureCmd.GainBlock(base.Owner,
base.DynamicVars.Block, null);
            }

            
await PowerCmd.Decrement(this);
        }
    }
public override string CustomPackedIconPath => "res://yuuki/images/powers/CatchSnowballPower.png";
public override string CustomBigIconPath => "res://yuuki/images/powers/CatchSnowballPower.png";
}




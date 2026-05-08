using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;

namespace yuuki.Scripts.Powers;

public class GatherSnowballPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            this.Flash();
            
            
            YukiCrystalSystem.AddCrystals(1);
            
            
            if (base.Amount > 1)
            {
                await PowerCmd.Decrement(this);
            }
            else
            {
                await PowerCmd.Remove(this);
            }
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/GatherSnowballPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/GatherSnowballPower.png";
}

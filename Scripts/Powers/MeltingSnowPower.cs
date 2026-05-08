using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public sealed class MeltingSnowPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            
            
            int drawAmount = (int)base.Amount * 2;
            await CardPileCmd.Draw(choiceContext, drawAmount, player);
            
            
            await PowerCmd.Remove(this);
        }
    }
    
    public override string CustomPackedIconPath => "res://yuuki/images/powers/MeltingSnowPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/MeltingSnowPower.png";
}

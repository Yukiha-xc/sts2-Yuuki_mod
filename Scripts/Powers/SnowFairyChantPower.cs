using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public sealed class SnowFairyChantPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            
            if (YukiCrystalSystem.CurrentCrystals >= 1)
            {
                YukiCrystalSystem.AddCrystals(-1);
                Flash();

                
                int cardsToDraw = (int)base.Amount;
                await CardPileCmd.Draw(choiceContext, cardsToDraw, player);
            }
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowFairyChantPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/SnowFairyChantPower.png";
}

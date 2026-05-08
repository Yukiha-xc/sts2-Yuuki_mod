using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using BaseLib.Abstracts;
using yuuki.Scripts;

namespace yuuki.Scripts.Powers;

public class FimbulwinterPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            
            
            YukiCrystalSystem.AddCrystals(1);
            
            
            int threshold = (int)base.Amount;
            if (YukiCrystalSystem.CurrentCrystals <= threshold)
            {
                YukiCrystalSystem.AddCrystals(1);
            }
        }
        await Task.CompletedTask;
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/fenbu.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/fenbu.png";
}

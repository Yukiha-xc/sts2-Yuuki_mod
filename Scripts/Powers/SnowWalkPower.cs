using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.ValueProps; 
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public class SnowWalkPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Side != side)
            return;

        int crystals = YukiCrystalSystem.CurrentCrystals;
        if (crystals > 0)
        {
            this.Flash(); 
            
            decimal totalBlock = (decimal)crystals * base.Amount;
            
            await CreatureCmd.GainBlock(
                Owner, 
                totalBlock, 
                ValueProp.Move, 
                null, 
                false
            );
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowWalkPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/SnowWalkPower.png";
}

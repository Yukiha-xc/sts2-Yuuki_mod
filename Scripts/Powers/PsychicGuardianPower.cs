using BaseLib.Abstracts;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Powers;
public sealed class PsychicGuardianPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        
        
        if (card.Owner.Creature == base.Owner && 
            (card.GetType().Name.Contains("Void") || card.Id.Entry == "Void"))
        {
            Flash();
            
            
            
            await PlayerCmd.GainEnergy((int)base.Amount, base.Owner.Player);
            
            
            await CardPileCmd.Draw(choiceContext, (int)base.Amount, base.Owner.Player);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/PsychicGuardianPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/PsychicGuardianPower.png";
}

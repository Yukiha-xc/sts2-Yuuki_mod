using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using BaseLib.Abstracts;
using yuuki.Scripts.Cards;

namespace yuuki.Scripts.Powers;

public class ActiveBodyPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount != 0)
        {
            
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, amount, applier, cardSource, silent: true);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side)
        {
            Flash();
            
            
            decimal amountToRemove = base.Amount;
            
            
            await PowerCmd.Remove(this);
            
            
            if (amountToRemove > 0)
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -amountToRemove, base.Owner, null);
            }
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/HDJT.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/HDJT.png";
}


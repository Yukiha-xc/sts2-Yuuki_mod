using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using BaseLib.Abstracts;
using yuuki.Scripts.Cards;

namespace yuuki.Scripts.Powers;

public class PrankPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, -amount, applier, cardSource, silent: true);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount != (decimal)base.Amount)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -amount, applier, cardSource, silent: true);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side)
        {
            Flash();
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, (decimal)base.Amount, base.Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/ezuoju.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/ezuoju.png";
}


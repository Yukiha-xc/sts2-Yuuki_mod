using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public sealed class BlackCatPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public decimal ReductionAmount { get; set; } = 6m;

    public BlackCatPower() { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Reduction", ReductionAmount)
    ];

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != base.Owner || amount <= 0 || base.Amount <= 0)
        {
            return amount;
        }

        
        decimal actualReduction = Math.Min(amount, ReductionAmount);
        Flash();
        return amount - actualReduction;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Player)
        {
            await PowerCmd.Decrement(this);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/BlackCatPower.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/BlackCatPower.png";
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;

namespace yuuki.Scripts.Powers;

public class SnowAbsorptionPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side)
        {
            Flash();
            
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, (decimal)(-base.Amount), base.Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public override string CustomPackedIconPath => "res://yuuki/images/powers/energy_icon_small.png";
    public override string CustomBigIconPath => "res://yuuki/images/powers/energy_icon_small.png";
}


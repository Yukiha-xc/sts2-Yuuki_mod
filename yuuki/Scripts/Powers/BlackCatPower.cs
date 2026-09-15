using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class BlackCatPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public decimal ReductionAmount { get; set; } = 6m;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Reduction", ReductionAmount)];

	public override string CustomPackedIconPath => "res://yuuki/images/powers/BlackCatPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/BlackCatPower.png";

	public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target != this.Owner || amount <= 0m || this.Amount <= 0)
		{
			return amount;
		}
		decimal num = Math.Min(amount, ReductionAmount);
		this.Flash();
		return amount - num;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == CombatSide.Player)
		{
			await PowerCmd.Decrement((PowerModel)(object)this);
		}
	}
}

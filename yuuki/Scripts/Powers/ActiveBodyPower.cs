using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace yuuki.Scripts.Powers;

public class ActiveBodyPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/HDJT.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/HDJT.png";

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if ((object)power == this && amount != 0m)
		{
			await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, amount, applier, cardSource, silent: true);
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == this.Owner.Side)
		{
			this.Flash();
			decimal amountToRemove = this.Amount;
			await PowerCmd.Remove((PowerModel?)(object)this);
			if (amountToRemove > 0m)
			{
				await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, -amountToRemove, this.Owner, null);
			}
		}
	}
}

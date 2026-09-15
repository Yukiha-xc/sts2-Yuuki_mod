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

public class PrankPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/ezuoju.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/ezuoju.png";

	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, -amount, applier, cardSource, silent: true);
	}


	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == this.Owner.Side)
		{
			this.Flash();
			await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
			await PowerCmd.Remove((PowerModel?)(object)this);
		}
	}
}

using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class PrayerPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/PrayerPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/PrayerPower.png";

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target != this.Owner || dealer == null)
		{
			return 1m;
		}
		if (!dealer.HasPower<EmpathyPower>())
		{
			return 1m;
		}
		return 0.5m;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == CombatSide.Enemy)
		{
			await PowerCmd.TickDownDuration((PowerModel)(object)this);
		}
	}
}

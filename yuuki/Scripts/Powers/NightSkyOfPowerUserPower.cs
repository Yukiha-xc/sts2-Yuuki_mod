using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class NightSkyOfPowerUserPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/NightSkyOfPowerUserPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/NightSkyOfPowerUserPower.png";

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		_ = this.Owner;
		return 1m;
	}
}

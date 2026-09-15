using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public class SnowWalkPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowWalkPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/SnowWalkPower.png";

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, System.Collections.Generic.IEnumerable<MegaCrit.Sts2.Core.Entities.Creatures.Creature> creatures)
	{
		if (side == this.Owner.Side)
		{
			int currentCrystals = YukiCrystalSystem.CurrentCrystals;
			if (currentCrystals > 0)
			{
				this.Flash();
				decimal amount = (decimal)currentCrystals * (decimal)this.Amount;
				await CreatureCmd.GainBlock(this.Owner, amount, ValueProp.Unpowered, null);
			}
		}
	}
}

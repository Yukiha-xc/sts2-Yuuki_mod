using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public class LonelySnowPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/LonelySnowPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/LonelySnowPower.png";

	public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		await base.AfterApplied(applier, cardSource);
		YukiCrystalSystem.OnCrystalGained -= HandleCrystalGained;
		YukiCrystalSystem.OnCrystalGained += HandleCrystalGained;
	}

	public override async Task AfterRemoved(Creature owner)
	{
		await base.AfterRemoved(owner);
		YukiCrystalSystem.OnCrystalGained -= HandleCrystalGained;
	}

	private async Task HandleCrystalGained(int amount)
	{
		if (amount > 0)
		{
			this.Flash();
			await TriggerBlock(amount);
		}
	}

	private async Task TriggerBlock(int crystalAmount)
	{
		await CreatureCmd.GainBlock(this.Owner, this.Amount * crystalAmount, ValueProp.Unpowered, null);
	}
}


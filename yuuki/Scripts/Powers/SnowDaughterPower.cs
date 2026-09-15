using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public class SnowDaughterPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowDaughterPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/SnowDaughterPower.png";

	public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		await base.AfterApplied(applier, cardSource);
		this.DisplayAmountChanged += UpdateMaxCrystals;
		UpdateMaxCrystals();
	}

	public override async Task AfterRemoved(Creature owner)
	{
		await base.AfterRemoved(owner);
		YukiCrystalSystem.MaxSnowCrystals = 9;
	}

	private void UpdateMaxCrystals()
	{
		YukiCrystalSystem.MaxSnowCrystals = 9 + this.Amount;
	}
}


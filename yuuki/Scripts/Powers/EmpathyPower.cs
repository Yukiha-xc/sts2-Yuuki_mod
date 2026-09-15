using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public class EmpathyPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.None;



	public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyPower.png";

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target == this.Owner)
		{
			int num = this.CombatState.Enemies.Count((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
			return 1.0m + 0.25m * (decimal)num;
		}
		return 1m;
	}

	public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
	{
		if (!(amount > 0m) || creature != this.Owner)
		{
			return;
		}
		Creature creature2 = this.CombatState.PlayerCreatures.FirstOrDefault();
		if (creature2 != null)
		{
			decimal num = 0.25m;
			decimal num2 = amount * num;
			if (num2 > 0m)
			{
				this.Flash();
				await PowerCmd.Apply<BlockNextTurnPower>(new ThrowingPlayerChoiceContext(), creature2, num2, creature2, null);
			}
		}
	}

	public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		await base.AfterApplied(applier, cardSource);
		await TriggerEmpathyInMemoryDamage(this.Owner);
	}

	public override async Task AfterRemoved(Creature owner)
	{
		await base.AfterRemoved(owner);
		YukiCrystalSystem.AddCrystals();
		await TriggerEmpathyInMemoryDamage(owner);
	}

	private async Task TriggerEmpathyInMemoryDamage(Creature target)
	{
		if (target == null || !target.IsAlive)
		{
			return;
		}
		Creature creature = this.CombatState.PlayerCreatures.FirstOrDefault();
		if (creature != null && creature.HasPower<EmpathyInMemoryPower>())
		{
			EmpathyInMemoryPower power = creature.GetPower<EmpathyInMemoryPower>();
			decimal amount = ((PowerModel)(object)power).DynamicVars.Damage.BaseValue * (decimal)((PowerModel)(object)power).Amount;
			if (target.IsAlive)
			{
				this.Flash();
				await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, amount, ValueProp.Move, creature, null);
			}
		}
	}
}

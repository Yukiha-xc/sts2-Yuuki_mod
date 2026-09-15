using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class DiaryPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];

	public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";

	public void SetDamage(decimal damage)
	{
		this.AssertMutable();
		this.DynamicVars.Damage.BaseValue = damage;
	}

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player)
		{
			if (this.Amount > 1)
			{
				await PowerCmd.Decrement((PowerModel)(object)this);
				return;
			}
			this.Flash();
			SfxCmd.Play("heavy_attack.mp3");
			await CreatureCmd.Damage(choiceContext, this.CombatState.HittableEnemies, this.DynamicVars.Damage, this.Owner);
			await PowerCmd.Remove((PowerModel?)(object)this);
		}
	}
}

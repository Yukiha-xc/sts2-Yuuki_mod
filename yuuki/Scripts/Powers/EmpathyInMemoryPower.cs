using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Powers;

public sealed class EmpathyInMemoryPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			List<IHoverTip> list = new List<IHoverTip>();
			list.AddRange(base.ExtraHoverTips);
			list.Add(HoverTipFactory.FromPower<EmpathyPower>());
			return list;
		}
	}

protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move)];

	public override string CustomPackedIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/EmpathyInMemoryPower.png";

	public override Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		if (cardSource != null && cardSource.IsUpgraded)
		{
			this.DynamicVars.Damage.UpgradeValueBy(3m);
		}
		return Task.CompletedTask;
	}

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player)
		{
			Creature? creature = this.CombatState?.Enemies.FirstOrDefault((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>());
			if (creature != null)
			{
				this.Flash();
				await PowerCmd.Remove<EmpathyPower>(creature);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Powers;

public sealed class SnowEmpathyPower : CustomPowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override string CustomPackedIconPath => "res://yuuki/images/powers/SnowEmpathyPower.png";

	public override string CustomBigIconPath => "res://yuuki/images/powers/SnowEmpathyPower.png";

	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player != this.Owner.Player)
		{
			return;
		}
		int triggerCount = this.Amount;
		for (int i = 0; i < triggerCount; i++)
		{
			List<Creature> hasEmpathy = this.CombatState.Enemies.Where((Creature e) => e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
			if (hasEmpathy.Count > 0)
			{
				this.Flash();
				Creature target = player.RunState.Rng.CombatTargets.NextItem(hasEmpathy);
				await PowerCmd.Remove<EmpathyPower>(target);
				YukiCrystalSystem.AddCrystals(2);
				await Cmd.Wait(0.1f);
			}
			else
			{
				List<Creature> noEmpathy = this.CombatState.Enemies.Where((Creature e) => e.IsAlive && !e.HasPower<EmpathyPower>()).ToList();
				if (noEmpathy.Count > 0 && YukiCrystalSystem.CurrentCrystals >= 2)
				{
					YukiCrystalSystem.AddCrystals(-2);
					this.Flash();
					Creature target = player.RunState.Rng.CombatTargets.NextItem(noEmpathy);
					await PowerCmd.Apply<EmpathyPower>(choiceContext, target, 1m, this.Owner, (CardModel?)null, false);
					await Cmd.Wait(0.1f);
				}
			}
		}
	}
}

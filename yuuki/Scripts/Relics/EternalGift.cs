using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class EternalGift : CustomRelicModel
{
	protected override string BigIconPath => "res://yuuki/images/relics/EternalGift.png";

	public override string PackedIconPath => "res://yuuki/images/relics/EternalGift.png";

	protected override string PackedIconOutlinePath => "res://yuuki/images/relics/EternalGift.png";

	public override RelicRarity Rarity => RelicRarity.Ancient;

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (dealer == null || !dealer.IsPlayer || target == null || target.IsPlayer)
		{
			return 1.0m;
		}
		int currentCrystals = YukiCrystalSystem.CurrentCrystals;
		if (currentCrystals < 3)
		{
			return 0.90m;
		}
		if (currentCrystals == 3)
		{
			return 1.0m;
		}
		return 1.0m + (decimal)(currentCrystals - 3) * 0.10m;
	}

	public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
	{
		int currentCrystals = YukiCrystalSystem.CurrentCrystals;
		if (currentCrystals < 3)
		{
			await CardPileCmd.Draw(choiceContext, 1m, player);
			YukiCrystalSystem.AddCrystals(2);
		}
		else
		{
			if (currentCrystals < 8)
			{
				return;
			}
			YukiCrystalSystem.AddCrystals(-2);
			CombatState combatState = CombatManager.Instance.DebugOnlyGetState();
			if (combatState == null)
			{
				return;
			}
			List<Creature> list = combatState.Enemies.Where((Creature e) => e.IsAlive).ToList();
			if (list.Count > 0)
			{
				List<Creature> list2 = list.Where((Creature e) => !e.HasPower<EmpathyPower>()).ToList();
				if (list2.Count == 0)
				{
					list2 = list;
				}
				Creature target = player.RunState.Rng.CombatTargets.NextItem(list2);
				await PowerCmd.Apply<EmpathyPower>((PlayerChoiceContext)new ThrowingPlayerChoiceContext(), target, 1m, player.Creature, (CardModel?)null, false);
			}
		}
	}

	public override async Task BeforeCombatStart()
	{
		YukiCrystalSystem.Reset();
		YukiCrystalSystem.AddCrystals(6);
		await Task.CompletedTask;
	}

	public EternalGift()
		: base(true)
	{
	}
}

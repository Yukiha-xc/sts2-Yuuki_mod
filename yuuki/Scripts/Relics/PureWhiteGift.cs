using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Relics;

[Pool(typeof(YukiRelicPool))]
public class PureWhiteGift : CustomRelicModel
{
	protected override string BigIconPath => "res://yuuki/images/relics/pure_white_gift.png";

	public override string PackedIconPath => "res://yuuki/images/relics/pure_white_gift.png";

	protected override string PackedIconOutlinePath => "res://yuuki/images/relics/pure_white_gift.png";

	public override RelicRarity Rarity => RelicRarity.Starter;

	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
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
		if (player != this.Owner)
		{
			return;
		}

		int currentCrystals = YukiCrystalSystem.CurrentCrystals;
		if (currentCrystals < 3)
		{
			await CardPileCmd.Draw(choiceContext, 1m, player);
			await YukiCrystalSystem.AddCrystals(2);
		}
		else
		{
			if (currentCrystals < 8)
			{
				return;
			}
			await YukiCrystalSystem.AddCrystals(-2);
			CombatState? combatState = CombatManager.Instance.DebugOnlyGetState();
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
				Creature? target = player.RunState.Rng.CombatTargets.NextItem(list2);
				if (target is not null)
				{
					await PowerCmd.Apply<EmpathyPower>(new ThrowingPlayerChoiceContext(), target, 1m, player.Creature, null, false);
				}
			}
		}
	}

	public override async Task BeforeCombatStart()
	{
		YukiCrystalSystem.Reset();
		await YukiCrystalSystem.AddCrystals(3);
	}

	public override RelicModel? GetUpgradeReplacement()
	{
		return (RelicModel?)(object)ModelDb.Relic<EternalGift>();
	}

	public PureWhiteGift()
		: base(true)
	{
	}
}

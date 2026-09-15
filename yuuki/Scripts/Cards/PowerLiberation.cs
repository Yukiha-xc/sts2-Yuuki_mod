using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PowerLiberation : YukiCardModel
{
	protected override bool HasEnergyCostX => true;

	public override bool GainsBlock => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(8m, ValueProp.Move),
		(DynamicVar)new BlockVar(4m, ValueProp.Move)
	});

	public PowerLiberation()
		: base(-1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		int x = this.ResolveEnergyXValue();
		if (x > 0)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).WithHitCount(x).FromCard(this, cardPlay)
				.TargetingAllOpponents(combatState)
				.WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				.Execute(choiceContext);
			for (int i = 0; i < x; i++)
			{
				await CreatureCmd.GainBlock(this.Owner.Creature, ((DynamicVar)this.DynamicVars.Block).BaseValue, ValueProp.Move, cardPlay, false);
				await Cmd.Wait(0.1f, false);
			}
			int overloadCount = Math.Min(x, 2);
			for (int i = 0; i < overloadCount; i++)
			{
				await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
			}
			await Cmd.Wait(0.25f, false);
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
		((DynamicVar)this.DynamicVars.Block).UpgradeValueBy(2m);
	}
}


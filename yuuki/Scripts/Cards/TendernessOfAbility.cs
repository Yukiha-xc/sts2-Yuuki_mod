using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class TendernessOfAbility : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/YUKI_h01a1.png";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[4]
	{
		(DynamicVar)new DamageVar(6m, ValueProp.Move),
		(DynamicVar)new CalculationBaseVar(0m),
		(DynamicVar)new CalculationExtraVar(1m),
		(DynamicVar)new CalculatedVar("Hits").WithMultiplier((CardModel card, Creature? _) => GetVoidCount(card))
	});

	public TendernessOfAbility()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		int voidCount = GetVoidCount((CardModel)this);
		if (voidCount > 0)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).WithHitCount(voidCount)
				.TargetingRandomOpponents(combatState, true)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}
		else
		{
			await Cmd.Wait(0.1f, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	private static int GetVoidCount(CardModel card)
	{
		CardPile pile = PileTypeExtensions.GetPile(PileType.Exhaust, card.Owner);
		if (pile == null)
		{
			return 0;
		}
		return pile.Cards.Count((CardModel c) => (int)c.Type == 4);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
	}
}

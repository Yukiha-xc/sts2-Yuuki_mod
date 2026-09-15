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
		(DynamicVar)new DamageVar(6m, (ValueProp)8),
		(DynamicVar)new CalculationBaseVar(0m),
		(DynamicVar)new CalculationExtraVar(1m),
		(DynamicVar)new CalculatedVar("Hits").WithMultiplier((Func<CardModel, Creature, decimal>)((CardModel card, Creature? _) => GetVoidCount(card)))
	});

	public TendernessOfAbility()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)4, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int voidCount = GetVoidCount((CardModel)this);
		if (voidCount > 0)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).WithHitCount(voidCount)
				.TargetingRandomOpponents(this.CombatState, true)
				.WithHitFx("vfx/vfx_attack_blunt", (string)null, "blunt_attack.mp3")
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
		CardPile pile = PileTypeExtensions.GetPile((PileType)4, card.Owner);
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

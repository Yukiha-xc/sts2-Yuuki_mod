using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MagicalGirl : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/MagicalGirl.jpg";

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[3]
	{
		(DynamicVar)new DamageVar(20m, ValueProp.Move),
		new DynamicVar("Heal", 2m),
		new DynamicVar("Gold", 15m)
	});

public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static((StaticHoverTip)6, Array.Empty<DynamicVar>())];

	public MagicalGirl()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		bool shouldTriggerFatal = cardPlay.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
		AttackCommand val = await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
			.Execute(choiceContext);
		if (shouldTriggerFatal && val.Results.SelectMany((List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled))
		{
			await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars["Heal"].BaseValue, true);
			await PlayerCmd.GainGold((decimal)(int)this.DynamicVars["Gold"].BaseValue, this.Owner, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Gold"].UpgradeValueBy(10m);
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}

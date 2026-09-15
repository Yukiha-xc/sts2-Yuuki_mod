using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class RabbitCleaver : YukiCardModel
{
	private int _reduction;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(17m, (ValueProp)8),
		(DynamicVar)new IntVar("Decrease", 5m)
	});

	public RabbitCleaver()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_attack_blunt", (string)null, "blunt_attack.mp3")
			.Execute(choiceContext);
		_reduction += this.DynamicVars["Decrease"].IntValue;
		UpdateDamage();
		await Cmd.Wait(0.25f, false);
	}

	private void UpdateDamage()
	{
		decimal num = (this.IsUpgraded ? 20m : 17m);
		((DynamicVar)this.DynamicVars.Damage).BaseValue = Math.Max(0m, num - (decimal)_reduction);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}

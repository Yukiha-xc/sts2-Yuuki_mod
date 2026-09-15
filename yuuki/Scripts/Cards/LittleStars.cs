using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class LittleStars : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/BG057_005.png";

	public override bool UsesEmpathy => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(4m, (ValueProp)8)];

	public LittleStars()
		: base(1, (CardType)1, (CardRarity)2, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		Creature target = cardPlay.Target;
		if (target == null)
		{
			return;
		}
		bool hasEmpathy = target.HasPower<EmpathyPower>();
		if (target.IsAlive)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).WithHitCount(2)
				.Targeting(target)
				.Execute(choiceContext);
		}
		if (hasEmpathy && target.IsAlive)
		{
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).WithHitCount(2)
				.Targeting(target)
				.Execute(choiceContext);
			if (target.IsAlive)
			{
				await PowerCmd.Remove<EmpathyPower>(target);
			}
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
	}
}

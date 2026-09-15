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
public class VibrateBlade : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(7m, (ValueProp)8)];

	public VibrateBlade()
		: base(2, (CardType)1, (CardRarity)3, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		decimal num = (await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
			.Execute(choiceContext)).Results.SelectMany((List<DamageResult> r) => r).Sum((DamageResult r) => r.UnblockedDamage);
		if (num > 0m)
		{
			await CreatureCmd.GainBlock(this.Owner.Creature, num, (ValueProp)8, cardPlay, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}

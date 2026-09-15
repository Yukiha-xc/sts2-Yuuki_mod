using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class HomingSnowball : YukiCardModel
{
	public override bool UsesSnowCrystals => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8m, ValueProp.Move), new DynamicVar("Vulnerable", 1m)];

	public HomingSnowball()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		Creature? target = cardPlay.Target;
		if (target != null)
		{
			if (YukiCrystalSystem.CurrentCrystals > 4)
			{
				await PowerCmd.Apply<VulnerablePower>(choiceContext, target, this.DynamicVars["Vulnerable"].BaseValue, this.Owner.Creature, (CardModel)this, false);
			}
			await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).Targeting(target)
				.Execute(choiceContext);
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
		this.DynamicVars["Vulnerable"].UpgradeValueBy(1m);
	}
}

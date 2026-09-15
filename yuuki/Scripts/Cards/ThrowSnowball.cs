using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ThrowSnowball : YukiCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(9m, (ValueProp)8),
		new DynamicVar("Weak", 1m)
	});

	public ThrowSnowball()
		: base(1, (CardType)1, (CardRarity)2, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.Execute(choiceContext);
		if (cardPlay.Target != null)
		{
			decimal baseValue = this.DynamicVars["Weak"].BaseValue;
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, baseValue, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
		this.DynamicVars["Weak"].UpgradeValueBy(1m);
	}
}

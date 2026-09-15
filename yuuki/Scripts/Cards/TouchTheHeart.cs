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
public class TouchTheHeart : YukiCardModel
{
	public override bool UsesEmpathy => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(4m, (ValueProp)8),
		new DynamicVar("Hits", 2m)
	});

	public TouchTheHeart()
		: base(1, (CardType)1, (CardRarity)3, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target == null)
		{
			return;
		}
		bool targetHadEmpathy = cardPlay.Target.HasPower<EmpathyPower>();
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).WithHitCount((int)this.DynamicVars["Hits"].BaseValue).FromCard((CardModel)this)
			.Targeting(cardPlay.Target)
			.Execute(choiceContext);
		if (!targetHadEmpathy)
		{
			return;
		}
		foreach (Creature hittableEnemy in this.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<EmpathyPower>(choiceContext, hittableEnemy, 1m, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Hits"].UpgradeValueBy(1m);
	}
}

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
		(DynamicVar)new DamageVar(4m, ValueProp.Move),
		new DynamicVar("Hits", 2m)
	});

	public TouchTheHeart()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target is not { } target || this.CombatState is not { } combatState)
		{
			return;
		}
		bool targetHadEmpathy = target.HasPower<EmpathyPower>();
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).WithHitCount((int)this.DynamicVars["Hits"].BaseValue).FromCard(this, cardPlay)
			.Targeting(target)
			.Execute(choiceContext);
		if (!targetHadEmpathy)
		{
			return;
		}
		foreach (Creature hittableEnemy in combatState.HittableEnemies)
		{
			await PowerCmd.Apply<EmpathyPower>(choiceContext, hittableEnemy, 1m, this.Owner.Creature, (CardModel)this, false);
		}
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["Hits"].UpgradeValueBy(1m);
	}
}

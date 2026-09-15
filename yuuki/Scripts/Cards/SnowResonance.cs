using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowResonance : YukiCardModel
{
	public override bool UsesEmpathy => true;

	public override bool UsesSnowCrystals => true;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(9m, (ValueProp)8),
		new YukiCrystalVar(1m)
	});

	public SnowResonance()
		: base(1, (CardType)1, (CardRarity)2, (TargetType)2, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Attack", this.Owner.Character.AttackAnimDelay);
		bool targetHadEmpathy = cardPlay.Target != null && cardPlay.Target.HasPower<EmpathyPower>();
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).Targeting(cardPlay.Target)
			.Execute(choiceContext);
		if (targetHadEmpathy)
		{
			YukiCrystalSystem.AddCrystals((int)this.DynamicVars["YukiCrystal"].BaseValue);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["YukiCrystal"].UpgradeValueBy(1m);
	}
}

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
public class Diary : YukiCardModel
{
protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new DamageVar(7m, (ValueProp)8)];

	public Diary()
		: base(1, (CardType)1, (CardRarity)2, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Attack", this.Owner.Character.AttackAnimDelay);
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
			.WithHitFx("vfx/vfx_attack_blunt", (string)null, "heavy_attack.mp3")
			.Execute(choiceContext);
		(await PowerCmd.Apply<DiaryPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false))?.SetDamage(((DynamicVar)this.DynamicVars.Damage).BaseValue);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
	}
}

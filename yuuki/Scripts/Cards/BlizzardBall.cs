using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class BlizzardBall : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/BlizzardSnowball.png";

	public override bool UsesSnowCrystals => true;

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new BlizzardBallDamageVar(7m)];

	public BlizzardBall()
		: base(2, (CardType)1, (CardRarity)2, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
			.WithHitFx("vfx/vfx_attack_blunt", (string)null, "heavy_attack.mp3")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(4m);
	}
}

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
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Prank : YukiCardModel
{
public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new DynamicVar("Weak", 1m),
		new DynamicVar("StrengthLoss", 4m)
	});

	public Prank()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
		decimal weakAmount = this.DynamicVars["Weak"].BaseValue;
		decimal strengthLoss = this.DynamicVars["StrengthLoss"].BaseValue;
		await PowerCmd.Apply<WeakPower>(choiceContext, this.Owner.Creature, weakAmount, this.Owner.Creature, (CardModel)this, false);
		await PowerCmd.Apply<PrankPower>(choiceContext, this.Owner.Creature, strengthLoss, this.Owner.Creature, (CardModel)this, false);
		foreach (Creature enemy in combatState.HittableEnemies)
		{
			await PowerCmd.Apply<WeakPower>(choiceContext, enemy, weakAmount, this.Owner.Creature, (CardModel)this, false);
			await PowerCmd.Apply<PrankPower>(choiceContext, enemy, strengthLoss, this.Owner.Creature, (CardModel)this, false);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["StrengthLoss"].UpgradeValueBy(2m);
	}
}

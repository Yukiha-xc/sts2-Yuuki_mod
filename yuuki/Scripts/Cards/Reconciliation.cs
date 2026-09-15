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
public class Reconciliation : YukiCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		new DynamicVar("BasePower", 1m),
		new DynamicVar("BonusPower", 2m)
	});

	public override bool UsesEmpathy => true;

	public Reconciliation()
		: base(1, (CardType)2, (CardRarity)4, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		decimal baseAmount = this.DynamicVars["BasePower"].BaseValue;
		decimal bonusAmount = this.DynamicVars["BonusPower"].BaseValue;
		foreach (Creature enemy in this.CombatState.HittableEnemies)
		{
			decimal totalAmount = baseAmount;
			if (enemy.HasPower<EmpathyPower>())
			{
				await PowerCmd.Remove<EmpathyPower>(enemy);
				totalAmount += bonusAmount;
			}
			if (enemy.IsAlive)
			{
				await PowerCmd.Apply<WeakPower>(choiceContext, enemy, totalAmount, this.Owner.Creature, (CardModel)this, false);
				await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, totalAmount, this.Owner.Creature, (CardModel)this, false);
			}
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["BonusPower"].UpgradeValueBy(1m);
	}
}

using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class AbilityLink : YukiCardModel
{
	public AbilityLink()
		: base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		this.ExhaustOnNextPlay = true;
		decimal num = default(decimal);
		decimal totalDex = default(decimal);
		foreach (Creature enemy in combatState.Enemies)
		{
			if (enemy.IsAlive && enemy.HasPower<EmpathyPower>())
			{
				StrengthPower? power = enemy.GetPower<StrengthPower>();
				if (power != null)
				{
					num += (decimal)((PowerModel)power).Amount;
				}
				DexterityPower? power2 = enemy.GetPower<DexterityPower>();
				if (power2 != null)
				{
					totalDex += (decimal)((PowerModel)power2).Amount;
				}
			}
		}
		await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, num + 1m, this.Owner.Creature, (CardModel)this, false);
		await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature, totalDex + 1m, this.Owner.Creature, (CardModel)this, false);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

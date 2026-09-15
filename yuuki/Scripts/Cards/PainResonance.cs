using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PainResonance : YukiCardModel
{
	public override bool UsesEmpathy => true;

	protected override bool ShouldGlowGoldInternal
	{
		get
		{
			ICombatState combatState = this.CombatState;
			if (combatState == null)
			{
				return false;
			}
			return combatState.HittableEnemies.Any((Creature e) => e.HasPower<EmpathyPower>());
		}
	}

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(8m, (ValueProp)8),
		(DynamicVar)new IntVar("WeakAmount", 1m)
	});

	public PainResonance()
		: base(1, (CardType)2, (CardRarity)3, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		List<Creature> empathyEnemies = this.CombatState.Enemies.Where((Creature e) => e != null && e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
		if (empathyEnemies.Count <= 0)
		{
			await Cmd.Wait(0.1f, false);
		}
		else
		{
			await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
			foreach (Creature enemy in empathyEnemies)
			{
				await PowerCmd.Remove<EmpathyPower>(enemy);
				if (enemy.IsAlive)
				{
					await PowerCmd.Apply<WeakPower>(choiceContext, enemy, this.DynamicVars["WeakAmount"].BaseValue, this.Owner.Creature, (CardModel)this, false);
					await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).WithHitCount(2).FromCard((CardModel)this)
						.Targeting(enemy)
						.Execute(choiceContext);
				}
			}
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["WeakAmount"].UpgradeValueBy(1m);
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(2m);
	}
}

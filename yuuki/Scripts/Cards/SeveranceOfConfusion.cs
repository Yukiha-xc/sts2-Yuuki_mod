using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SeveranceOfConfusion : YukiCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>((DynamicVar[])(object)new DynamicVar[2]
	{
		(DynamicVar)new DamageVar(15m, ValueProp.Move),
		new DynamicVar("Power", 1m)
	});

	public override int CapacityOverload => 1;

	public SeveranceOfConfusion()
		: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		foreach (Creature enemy in combatState.Enemies)
		{
			if (enemy.IsAlive)
			{
				if (enemy.Block > 0)
				{
					await CreatureCmd.LoseBlock(choiceContext, enemy, enemy.Block, this.Owner.Creature);
				}
				await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, this.DynamicVars["Power"].BaseValue, this.Owner.Creature, (CardModel)this, false);
			}
		}
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(combatState)
			.Execute(choiceContext);
		await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}


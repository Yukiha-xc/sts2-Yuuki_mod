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
		(DynamicVar)new DamageVar(15m, (ValueProp)8),
		new DynamicVar("Power", 1m)
	});

	public override int CapacityOverload => 1;

	public SeveranceOfConfusion()
		: base(2, (CardType)1, (CardRarity)3, (TargetType)3, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		foreach (Creature enemy in this.CombatState.Enemies)
		{
			if (enemy.IsAlive)
			{
				if (enemy.Block > 0)
				{
					await CreatureCmd.LoseBlock(enemy, (decimal)enemy.Block);
				}
				await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, this.DynamicVars["Power"].BaseValue, this.Owner.Creature, (CardModel)this, false);
			}
		}
		await DamageCmd.Attack(((DynamicVar)this.DynamicVars.Damage).BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState)
			.Execute(choiceContext);
		await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), (PileType)3, (Player)null, (CardPilePosition)1);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Damage).UpgradeValueBy(3m);
	}
}


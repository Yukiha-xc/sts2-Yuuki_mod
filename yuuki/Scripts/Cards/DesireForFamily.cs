using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class DesireForFamily : YukiCardModel
{
	public override int CapacityOverload => 1;

public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new CardsVar(2)];

	public DesireForFamily()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.None, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await CardPileCmd.Draw(choiceContext, ((DynamicVar)this.DynamicVars.Cards).BaseValue, this.Owner, false);
		List<Creature> list = combatState.Enemies.Where((Creature e) => e.IsAlive).ToList();
		if (list.Count > 0)
		{
			Creature val = list[this.Owner.RunState.Rng.Shuffle.NextInt(0, list.Count)];
			await PowerCmd.Apply<EmpathyPower>(choiceContext, val, 1m, this.Owner.Creature, (CardModel)this, false);
		}
		await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Cards).UpgradeValueBy(1m);
	}
}

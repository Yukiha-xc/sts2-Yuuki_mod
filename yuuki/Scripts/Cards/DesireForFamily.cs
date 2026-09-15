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

public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar)new CardsVar(2)];

	public DesireForFamily()
		: base(0, (CardType)2, (CardRarity)2, (TargetType)0, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CardPileCmd.Draw(choiceContext, ((DynamicVar)this.DynamicVars.Cards).BaseValue, this.Owner, false);
		List<Creature> list = this.CombatState.Enemies.Where((Creature e) => e.IsAlive).ToList();
		if (list.Count > 0)
		{
			Creature val = list[this.Owner.RunState.Rng.Shuffle.NextInt(0, list.Count)];
			await PowerCmd.Apply<EmpathyPower>(choiceContext, val, 1m, this.Owner.Creature, (CardModel)this, false);
		}
		await CardPileCmd.AddGeneratedCardToCombat(this.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), (PileType)3, (MegaCrit.Sts2.Core.Entities.Players.Player)null, (CardPilePosition)1);
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		((DynamicVar)this.DynamicVars.Cards).UpgradeValueBy(1m);
	}
}

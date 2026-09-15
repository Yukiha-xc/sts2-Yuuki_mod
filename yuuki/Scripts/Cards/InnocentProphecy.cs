using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class InnocentProphecy : YukiCardModel
{
	public override int CapacityOverload => (int)this.DynamicVars["CapacityOverload"].BaseValue;

protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CapacityOverload", 2m)];

	public InnocentProphecy()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		await CardPileCmd.ShuffleIfNecessary(choiceContext, this.Owner);
		IReadOnlyList<CardModel> cards = PileTypeExtensions.GetPile(PileType.Draw, this.Owner).Cards;
		if (cards.Any())
		{
			CardSelectorPrefs val = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
			IEnumerable<CardModel> source = await CardSelectCmd.FromSimpleGrid(choiceContext, cards.ToList(), this.Owner, val);
			CardModel? selected = source.FirstOrDefault();
			if (selected is not null)
			{
				await CardPileCmd.Add(selected, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}
		await PowerCmd.Apply<InnocentProphecyPower>(choiceContext, this.Owner.Creature, 1m, this.Owner.Creature, (CardModel)this, false);
		int overloadCount = CapacityOverload;
		for (int i = 0; i < overloadCount; i++)
		{
			await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
		}
		await Cmd.Wait(0.25f, false);
	}

	protected override void OnUpgrade()
	{
		this.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
	}
}




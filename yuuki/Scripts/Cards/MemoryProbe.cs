using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MemoryProbe : YukiCardModel
{
	public override string PortraitPath => "res://yuuki/images/cards/BG096_000.png";

	public override int CapacityOverload => 1;

	public override IEnumerable<CardKeyword> CanonicalKeywords => (IEnumerable<CardKeyword>)(object)new CardKeyword[1] { CardKeyword.Exhaust };

	public MemoryProbe()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		this.ExhaustOnNextPlay = true;
		CardPile pile = PileTypeExtensions.GetPile(PileType.Exhaust, this.Owner);
		if (pile.Cards.Count > 0)
		{
			CardSelectorPrefs val = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
			CardModel? val2 = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, this.Owner, val)).FirstOrDefault();
			if (val2 != null)
			{
				await CardPileCmd.Add(val2, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}
		await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(this.Owner), PileType.Discard, null, CardPilePosition.Bottom);
	}

	protected override void OnUpgrade()
	{
		this.EnergyCost.SetCustomBaseCost(0);
	}
}




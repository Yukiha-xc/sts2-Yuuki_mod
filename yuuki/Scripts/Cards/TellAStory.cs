using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class TellAStory : YukiCardModel
{
public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

	public override string PortraitPath => "res://yuuki/images/cards/Storytelling.png";

	public TellAStory()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (this.CombatState is not { } combatState)
		{
			return;
		}

		CardModel item = combatState.CreateCard<SpecialMemory>(this.Owner);
		CardModel item2 = combatState.CreateCard<SpecialStarrySky>(this.Owner);
		CardModel item3 = combatState.CreateCard<SpecialBlackCat>(this.Owner);
		List<CardModel> list = new List<CardModel> { item, item2, item3 };
		if (this.IsUpgraded)
		{
			CardCmd.Upgrade((IEnumerable<CardModel>)list, CardPreviewStyle.HorizontalLayout);
		}
		CardModel? val = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, this.Owner, false);
		if (val != null)
		{
			await CardPileCmd.AddGeneratedCardToCombat(val, PileType.Hand, null, CardPilePosition.Bottom);
		}
	}

	protected override void OnUpgrade()
	{
	}
}

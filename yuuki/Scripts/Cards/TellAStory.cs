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
public override IEnumerable<CardKeyword> CanonicalKeywords => [(CardKeyword)1];

	public override string PortraitPath => "res://yuuki/images/cards/Storytelling.png";

	public TellAStory()
		: base(1, (CardType)2, (CardRarity)4, (TargetType)1, shouldShowInCardLibrary: true)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		CardModel item = (CardModel)this.CombatState.CreateCard<SpecialMemory>(this.Owner);
		CardModel item2 = (CardModel)this.CombatState.CreateCard<SpecialStarrySky>(this.Owner);
		CardModel item3 = (CardModel)this.CombatState.CreateCard<SpecialBlackCat>(this.Owner);
		List<CardModel> list = new List<CardModel> { item, item2, item3 };
		if (this.IsUpgraded)
		{
			CardCmd.Upgrade((IEnumerable<CardModel>)list, (CardPreviewStyle)1);
		}
		CardModel val = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>)list, this.Owner, false);
		if (val != null)
		{
			await CardPileCmd.AddGeneratedCardToCombat(val, (PileType)2, (Player)null, (CardPilePosition)1);
		}
	}

	protected override void OnUpgrade()
	{
	}
}

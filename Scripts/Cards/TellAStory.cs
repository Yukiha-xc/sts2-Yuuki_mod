using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class TellAStory : YukiCardModel
{
    public TellAStory() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override string PortraitPath => "res://yuuki/images/cards/Storytelling.png";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel optMemory = base.CombatState.CreateCard<SpecialMemory>(base.Owner);
        CardModel optStarrySky = base.CombatState.CreateCard<SpecialStarrySky>(base.Owner);
        CardModel optBlackCat = base.CombatState.CreateCard<SpecialBlackCat>(base.Owner);

        List<CardModel> choices = new List<CardModel> { optMemory, optStarrySky, optBlackCat };
        
        if (base.IsUpgraded)
        {
            CardCmd.Upgrade(choices, MegaCrit.Sts2.Core.Nodes.CommonUi.CardPreviewStyle.HorizontalLayout);
        }
        
        
        CardModel selected = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, base.Owner, canSkip: false);
        
        if (selected != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(selected, PileType.Hand, null);
        }
    }

    protected override void OnUpgrade()
    {
    }
}

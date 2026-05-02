using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;
using BaseLib.Utils;
using yuuki.Scripts;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialMemory : YukiCardModel
{
    public SpecialMemory() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true) 
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    public override string PortraitPath => "res://yuuki/images/cards/memory.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Draw", 4m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, (int)base.DynamicVars["Draw"].BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Draw"].UpgradeValueBy(2m);
    }
}


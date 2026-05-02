using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EndOfWandering : YukiCardModel
{
    
    public EndOfWandering() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) 
    {
    }

    public override int CapacityOverload => (int)base.DynamicVars["CapacityOverload"].BaseValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("CapacityOverload", 2m)
    ];

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        var handCards = PileType.Hand.GetPile(base.Owner).Cards;

        foreach (CardModel item in handCards)
        {
            
            item.AddKeyword(CardKeyword.Retain);
        }

        
        int overloadCount = CapacityOverload;
        for (int i = 0; i < overloadCount; i++)
        {
            CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, true);
        }
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
    }
}

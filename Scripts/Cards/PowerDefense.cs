using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PowerDefense : YukiCardModel
{
    
    public PowerDefense() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true) 
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        var handCards = PileType.Hand.GetPile(base.Owner).Cards;
        var statusCards = handCards.Where(c => c.Type == CardType.Status).ToList();

        int count = statusCards.Count;

        if (count > 0)
        {
            foreach (var card in statusCards)
            {
                
                await CardPileCmd.Add(card, PileType.Exhaust);
            }

            
            await CardPileCmd.Draw(choiceContext, count, base.Owner);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        this.AddKeyword(CardKeyword.Retain);
    }
}

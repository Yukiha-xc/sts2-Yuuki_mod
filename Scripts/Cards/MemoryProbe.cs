using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MemoryProbe : YukiCardModel
{
    
    public MemoryProbe() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }
 
    public override string PortraitPath => "res://yuuki/images/cards/BG096_000.png";

    
    public override int CapacityOverload => 1;

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        this.ExhaustOnNextPlay = true;

        CardPile exhaustPile = PileType.Exhaust.GetPile(base.Owner);
        
        if (exhaustPile.Cards.Count > 0)
        {
            
            CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
            
            
            var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, exhaustPile.Cards, base.Owner, prefs);
            CardModel selected = selectedCards.FirstOrDefault();
            
            if (selected != null)
            {
                
                await CardPileCmd.Add(selected, PileType.Hand);
            }
        }
        
        
        CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
    }

    protected override void OnUpgrade()
    {
        
        this.EnergyCost.SetCustomBaseCost(0);
    }
}


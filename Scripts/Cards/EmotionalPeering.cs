using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class EmotionalPeering : YukiCardModel
{
    
    public EmotionalPeering() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true) 
    {
    }

    public override bool UsesEmpathy => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        var cardsToDiscard = await CardSelectCmd.FromHandForDiscard(
            choiceContext, 
            base.Owner, 
            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), 
            null, 
            this
        );
        await CardCmd.Discard(choiceContext, cardsToDiscard);

        
        if (cardPlay.Target != null)
        {
            
            await PowerCmd.Apply<EmpathyPower>(cardPlay.Target, 1m, base.Owner.Creature, this);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.EnergyCost.UpgradeBy(-1);
    }
}


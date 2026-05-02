using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class FoundYou : YukiCardModel
{
    public FoundYou() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true) { }

    public override bool UsesEmpathy => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Discard", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, 2m, base.Owner);
        
        int discardAmount = (int)DynamicVars["Discard"].BaseValue;
        
        var selectedCards = (await CardSelectCmd.FromHandForDiscard(
            choiceContext, 
            base.Owner, 
            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, discardAmount), 
            null, 
            this
        )).ToList();

        if (selectedCards.Count > 0)
        {
            await CardCmd.Discard(choiceContext, selectedCards);
            
            
            if (selectedCards.All(c => c.Type == CardType.Skill) && cardPlay.Target != null)
            {
                await PowerCmd.Apply<EmpathyPower>(cardPlay.Target, 1m, base.Owner.Creature, this);
            }
        }
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Discard"].UpgradeValueBy(-1m);
    }
}

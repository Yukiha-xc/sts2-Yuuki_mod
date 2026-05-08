using System.Collections.Generic;
using System.Linq;
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
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class InnocentProphecy : YukiCardModel
{
    public InnocentProphecy() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true) { }
    
    public override int CapacityOverload => (int)base.DynamicVars["CapacityOverload"].BaseValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("CapacityOverload", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner);
        var drawPile = PileType.Draw.GetPile(base.Owner).Cards;
        
        if (drawPile.Any())
        {
            CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
            var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, drawPile.ToList(), base.Owner, prefs);
            if (selectedCards.Any())
            {
                await CardPileCmd.Add(selectedCards.First(), PileType.Hand);
            }
        }

        
        await PowerCmd.Apply<InnocentProphecyPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);

        
        int overloadCount = CapacityOverload;
        for (int i = 0; i < overloadCount; i++)
        {
            CardModel voidCard = base.CombatState.CreateCard<MegaCrit.Sts2.Core.Models.Cards.Void>(base.Owner);
            await CardPileCmd.AddGeneratedCardToCombat(voidCard, PileType.Discard, null);
        }
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["CapacityOverload"].UpgradeValueBy(-1m);
    }
}



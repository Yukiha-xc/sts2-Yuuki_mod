using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ActingCoquettishly : YukiCardModel
{
    
    public ActingCoquettishly() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) 
    {
        
    }

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hand = PileType.Hand.GetPile(base.Owner).Cards;
        
        var otherCards = hand.Where(c => c != null && c != this).ToList();

        if (otherCards.Count > 0)
        {
            var selectedCard = otherCards[base.Owner.RunState.Rng.Shuffle.NextInt(0, otherCards.Count)];
            
            Creature? target = null;
            if (selectedCard.TargetType == TargetType.AnyEnemy)
            {
                var aliveEnemies = base.CombatState.Enemies.Where(e => e.IsAlive).ToList();
                if (aliveEnemies.Count > 0)
                {
                    target = aliveEnemies[base.Owner.RunState.Rng.Shuffle.NextInt(0, aliveEnemies.Count)];
                }
            }

            await CardCmd.AutoPlay(choiceContext, selectedCard, target);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

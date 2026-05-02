using System;
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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class CherishedMemories : YukiCardModel
{
    public CherishedMemories() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true) { }

    public override bool UsesEmpathy => true;

    protected override bool ShouldGlowGoldInternal => base.CombatState != null && base.CombatState.Enemies.Any(e => e != null && e.IsAlive && e.HasPower<EmpathyPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await MegaCrit.Sts2.Core.Commands.DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (cardPlay.Target.HasPower<EmpathyPower>())
        {
            CardPile discardPile = PileType.Discard.GetPile(base.Owner);
            var cardsInDiscard = discardPile.Cards.ToList();

            if (cardsInDiscard.Count > 0)
            {
                var prompt = new MegaCrit.Sts2.Core.Localization.LocString("ui", "TEXT_SELECT_CARD");
                CardSelectorPrefs prefs = new CardSelectorPrefs(prompt, 1);
                var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, cardsInDiscard, base.Owner, prefs);
                
                if (selectedCards != null && selectedCards.Any())
                {
                    await CardPileCmd.Add(selectedCards.First(), PileType.Hand);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}

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
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class DesireForFamily : YukiCardModel
{
    public DesireForFamily() : base(0, CardType.Skill, CardRarity.Common, TargetType.None, true) { }

    public override int CapacityOverload => 1;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);

        
        var aliveEnemies = base.CombatState.Enemies.Where(e => e.IsAlive).ToList();
        if (aliveEnemies.Count > 0)
        {
            var target = aliveEnemies[base.Owner.RunState.Rng.Shuffle.NextInt(0, aliveEnemies.Count)];
            await PowerCmd.Apply<EmpathyPower>(choiceContext, target, 1m, base.Owner.Creature, this);
        }
        
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using BaseLib.Abstracts;
using BaseLib.Utils;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialStarrySky : YukiCardModel
{
    public SpecialStarrySky() : base(0, CardType.Skill, CardRarity.Token, TargetType.AllEnemies, true) 
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    public override string PortraitPath => "res://yuuki/images/cards/StarrySky.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Doom", 15m),
        new IntVar("Energy", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<DoomPower>(choiceContext, enemy, base.DynamicVars["Doom"].BaseValue, base.Owner.Creature, this);
        }

        
        await PlayerCmd.GainEnergy((int)base.DynamicVars["Energy"].BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Doom"].UpgradeValueBy(5m);
        base.DynamicVars["Energy"].UpgradeValueBy(1m);
    }
}





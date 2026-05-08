using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class PsychicGuardian : YukiCardModel
{
    
    public PsychicGuardian() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Draw", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<PsychicGuardianPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Draw"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Draw"].UpgradeValueBy(1m);
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class MagicOfFamily : YukiCardModel
{
    
    public MagicOfFamily() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) 
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Heal", 4m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars["Heal"].BaseValue, true);

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Heal"].UpgradeValueBy(2m);
    }
}

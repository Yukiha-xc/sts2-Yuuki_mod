using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowDaughter : YukiCardModel
{
    public SnowDaughter() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<yuuki.Scripts.Powers.SnowDaughterPower>("MagicNumber", 2m) 
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        
        await PowerCmd.Apply<yuuki.Scripts.Powers.SnowDaughterPower>(
            base.Owner.Creature, 
            base.DynamicVars["MagicNumber"].BaseValue, 
            base.Owner.Creature, 
            this
        );
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["MagicNumber"].UpgradeValueBy(2m);
    }
}

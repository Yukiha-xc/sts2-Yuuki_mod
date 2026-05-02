using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ActiveBody : YukiCardModel
{
    
    public ActiveBody() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Strength", 1m),
        new DynamicVar("TempStrength", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        
        bool hasStrength = base.Owner.Creature.GetPowerAmount<StrengthPower>() > 0;

        if (hasStrength)
        {
            
            await PowerCmd.Apply<ActiveBodyPower>(
                base.Owner.Creature, 
                base.DynamicVars["TempStrength"].BaseValue, 
                base.Owner.Creature, 
                this
            );
        }

        
        
        await PowerCmd.Apply<StrengthPower>(
            base.Owner.Creature, 
            base.DynamicVars["Strength"].BaseValue, 
            base.Owner.Creature, 
            this
        );

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Strength"].UpgradeValueBy(1m);
        base.DynamicVars["TempStrength"].UpgradeValueBy(1m);
    }
}

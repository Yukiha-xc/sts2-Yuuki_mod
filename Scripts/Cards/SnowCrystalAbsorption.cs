using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCrystalAbsorption : YukiCardModel
{
    public SnowCrystalAbsorption() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("YukiConsume", 2m),
        new DynamicVar("EnergyGain", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int consumeAmount = (int)base.DynamicVars["YukiConsume"].BaseValue;
        
        if (YukiCrystalSystem.CurrentCrystals >= consumeAmount)
        {
            
            YukiCrystalSystem.AddCrystals(-consumeAmount);
            
            
            int energyAmount = (int)base.DynamicVars["EnergyGain"].BaseValue;
            await PlayerCmd.GainEnergy(energyAmount, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["EnergyGain"].UpgradeValueBy(1m);
    }
}

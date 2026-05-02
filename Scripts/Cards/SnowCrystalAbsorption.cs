using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
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
        new DynamicVar("Strength", 2m),
        new DynamicVar("YukiConsume", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (YukiCrystalSystem.CurrentCrystals >= 2)
        {
            YukiCrystalSystem.AddCrystals(-2);
            await PlayerCmd.GainEnergy(2, base.Owner);
            
            decimal strengthAmount = DynamicVars["Strength"].BaseValue;
            
            await PowerCmd.Apply<StrengthPower>(base.Owner.Creature, strengthAmount, base.Owner.Creature, this);
            
            await PowerCmd.Apply<SnowAbsorptionPower>(base.Owner.Creature, strengthAmount, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Strength"].UpgradeValueBy(2m);
    }
}

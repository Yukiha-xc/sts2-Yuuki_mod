using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class FallingSnow : YukiCardModel
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.None;
    private const bool shouldShowInCardLibrary = true;

    public override bool UsesSnowCrystals => true;

    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new YukiCrystalVar(2)];

    public FallingSnow() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        int amount = (int)DynamicVars[YukiCrystalVar.Key].BaseValue;
        YukiCrystalSystem.AddCrystals(amount);
        
        await Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        
        DynamicVars[YukiCrystalVar.Key].UpgradeValueBy(1);
    }
}

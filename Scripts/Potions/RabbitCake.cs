using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using yuuki.Scripts;

namespace yuuki.Scripts.Potions;

[Pool(typeof(YukiPotionPool))]
public class RabbitCake : CustomPotionModel
{
    
    public override PotionRarity Rarity => PotionRarity.Common;

    
    public override PotionUsage Usage => PotionUsage.CombatOnly;

    
    public override TargetType TargetType => TargetType.Self;

    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new YukiCrystalVar(3)];

    
    public override string? CustomPackedImagePath => "res://yuuki/images/potions/rabbit_cake.png";
    public override string? CustomPackedOutlinePath => "res://yuuki/images/potions/rabbit_cake.png";

    
    public override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            foreach (var tip in base.ExtraHoverTips) yield return tip;
            
        }
    }

    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        
        YukiCrystalSystem.AddCrystals(DynamicVars[YukiCrystalVar.Key].IntValue);
        
        await Task.CompletedTask;
    }
}

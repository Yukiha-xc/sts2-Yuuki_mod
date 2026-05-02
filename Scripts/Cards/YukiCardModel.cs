using System.Collections.Generic;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace yuuki.Scripts.Cards;


public abstract class YukiCardModel : CustomCardModel
{
    
    public override string PortraitPath => $"res://yuuki/images/cards/{GetType().Name}.png";

    
    public virtual int CapacityOverload => 0;

    
    public virtual bool UsesSnowCrystals => false;

    
    public virtual bool UsesEmpathy => false;

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            
            if (CapacityOverload > 0)
            {
                yield return new HoverTip(
                    new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.title"),
                    new LocString("static_hover_tips", "YUUKI_CAPACITY_OVERLOAD.description"),
                    null
                );
                yield return HoverTipFactory.FromCard<MegaCrit.Sts2.Core.Models.Cards.Void>();
            }

            
            if (UsesSnowCrystals)
            {
                yield return new HoverTip(
                    new LocString("static_hover_tips", "YUUKI_SNOW_CRYSTAL.title"),
                    new LocString("static_hover_tips", "YUUKI_SNOW_CRYSTAL.description"),
                    null
                );
            }

            
            if (UsesEmpathy)
            {
                yield return new HoverTip(
                    new LocString("static_hover_tips", "YUUKI_EMPATHY.title"),
                    new LocString("static_hover_tips", "YUUKI_EMPATHY.description"),
                    null
                );
            }

            
            if (GainsBlock)
            {
                yield return HoverTipFactory.Static(StaticHoverTip.Block);
            }
        }
    }

    public YukiCardModel(int energyCost, CardType type, CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary) 
        : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}

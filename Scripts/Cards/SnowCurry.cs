using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowCurry : YukiCardModel
{
    
    public SnowCurry() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    public override string PortraitPath => "res://yuuki/images/cards/ETC_FD_e004a.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Strength", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (YukiCrystalSystem.CurrentCrystals >= 2)
        {
            YukiCrystalSystem.AddCrystals(-2);
            
            
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(
                base.Owner.Creature, 
                base.DynamicVars["Strength"].BaseValue, 
                base.Owner.Creature, 
                this
            );
        }
    }

    
    protected override bool IsPlayable => YukiCrystalSystem.CurrentCrystals >= 2;

    
    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Strength"].UpgradeValueBy(1m);
    }
}

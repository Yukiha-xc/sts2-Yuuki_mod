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
        new DynamicVar("YukiConsume", 3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int consumeAmount = (int)base.DynamicVars["YukiConsume"].BaseValue;
        if (YukiCrystalSystem.CurrentCrystals >= consumeAmount)
        {
            YukiCrystalSystem.AddCrystals(-consumeAmount);
            
            
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(choiceContext, base.Owner.Creature, 
                1m, 
                base.Owner.Creature, 
                this
            );
            
            
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.DexterityPower>(choiceContext, base.Owner.Creature, 
                1m, 
                base.Owner.Creature, 
                this
            );
        }
    }

    protected override bool IsPlayable => YukiCrystalSystem.CurrentCrystals >= (int)base.DynamicVars["YukiConsume"].BaseValue;

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["YukiConsume"].UpgradeValueBy(-1m);
    }
}

